using HotelCRM.Business.Assignment;
using HotelCRM.Business.Factories;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Services;

/// <summary>
/// UC2 to UC5: create, assign, process and search tickets. The service only looks data up and passes
/// the results to domain methods; every business error is thrown by a domain class. It reaches the
/// customer subsystem only through <see cref="ICustomerDirectory"/>, <see cref="IStayLookup"/>,
/// <see cref="IAssignmentStrategy"/> and the attached <see cref="ITicketObserver"/> objects.
/// </summary>
public sealed class TicketService
{
    private readonly IRepository<SupportTicket> _tickets;
    private readonly IRepository<Employee> _employees;
    private readonly ICustomerDirectory _customers;
    private readonly IStayLookup _stays;
    private readonly IAssignmentStrategy _strategy;
    private readonly IReadOnlyDictionary<string, TicketFactory> _factories;
    private readonly IReadOnlyList<ITicketObserver> _observers;
    private readonly BusinessSettings _settings;

    /// <summary>Creates the service and attaches every observer to every ticket already in the repository.</summary>
    /// <param name="tickets">Ticket repository.</param>
    /// <param name="employees">Employee repository; agents are the employees of type <see cref="Agent"/>.</param>
    /// <param name="customers">Customer lookups of the other subsystem.</param>
    /// <param name="stays">Stay lookups of the other subsystem.</param>
    /// <param name="strategy">Assignment algorithm used by <see cref="AutoAssign"/>.</param>
    /// <param name="factories">Lookup table: ticket type name (<see cref="SupportTicket.TypeName"/>) to factory.</param>
    /// <param name="observers">Observers attached to every loaded and every new ticket.</param>
    /// <param name="settings">Limits: <see cref="BusinessSettings.MaxActiveTickets"/> and <see cref="BusinessSettings.ReopenDays"/>.</param>
    public TicketService(
        IRepository<SupportTicket> tickets,
        IRepository<Employee> employees,
        ICustomerDirectory customers,
        IStayLookup stays,
        IAssignmentStrategy strategy,
        IReadOnlyDictionary<string, TicketFactory> factories,
        IReadOnlyList<ITicketObserver> observers,
        BusinessSettings settings)
    {
        _tickets = tickets;
        _employees = employees;
        _customers = customers;
        _stays = stays;
        _strategy = strategy;
        _factories = factories;
        _observers = observers;
        _settings = settings;

        foreach (SupportTicket ticket in _tickets.GetAll())
        {
            AttachObservers(ticket);
        }
    }

    /// <summary>
    /// UC2: creates a ticket. Both roles may do this (rule 11), so there is no actor.
    /// Steps: get the SLA profile from <see cref="ICustomerDirectory.GetSlaProfile"/> (rule 4); take the
    /// factory for <paramref name="typeName"/> from the table; take the next Id; let the factory create
    /// the ticket; ask <see cref="IStayLookup.IsRoomInActiveStay"/> (false when <paramref name="roomId"/>
    /// is null) and call <see cref="SupportTicket.Validate"/> with the answer (rule 5); attach the
    /// observers; save.
    /// </summary>
    /// <param name="typeName">One of the keys returned by <see cref="GetTicketTypes"/>.</param>
    /// <param name="customerId">Id of the customer.</param>
    /// <param name="title">Short title; required.</param>
    /// <param name="description">Details; required for complaints.</param>
    /// <param name="priority">Priority before the tier boost.</param>
    /// <param name="roomId">Room the ticket is about; required for maintenance tickets, otherwise null.</param>
    /// <param name="now">Current time, supplied by the form.</param>
    /// <returns>The saved ticket.</returns>
    /// <exception cref="ValidationException">Empty title, empty complaint description or missing maintenance room.</exception>
    /// <exception cref="BusinessRuleException">The customer does not exist, or the room is not part of an active stay of the customer (rule 5).</exception>
    /// <exception cref="KeyNotFoundException">No factory is registered for <paramref name="typeName"/> (a programming error).</exception>
    public SupportTicket CreateTicket(
        string typeName,
        int customerId,
        string title,
        string description,
        TicketPriority priority,
        int? roomId,
        DateTime now)
    {
        CustomerSlaProfile slaProfile = _customers.GetSlaProfile(customerId);
        TicketFactory factory = _factories[typeName];
        int id = _tickets.NextId();
        SupportTicket ticket = factory.Create(id, customerId, title, description, priority, slaProfile, now, roomId);

        bool roomInActiveStay = roomId is not null && _stays.IsRoomInActiveStay(customerId, roomId.Value);
        ticket.Validate(roomInActiveStay);

        AttachObservers(ticket);
        _tickets.Save(ticket);
        return ticket;
    }

    /// <summary>
    /// UC3: lets the configured strategy choose an agent and assigns the ticket to them.
    /// Steps: <see cref="Employee.EnsureCanAssign"/> on the actor (rule 11); count the active tickets of
    /// every agent; keep the agents for whom <see cref="Agent.CanTake"/> is true (rule 3) as candidates;
    /// call <see cref="IAssignmentStrategy.Pick"/> with <see cref="SupportTicket.HandlingDepartmentCode"/>;
    /// call <see cref="SupportTicket.AssignTo"/> with the result (null when nobody is available); save.
    /// </summary>
    /// <returns>The agent the ticket is now assigned to.</returns>
    /// <exception cref="BusinessRuleException">
    /// The actor is not a manager, the ticket does not exist, the ticket is Resolved or Closed, or no agent is available.
    /// </exception>
    public Agent AutoAssign(Employee actor, int ticketId)
    {
        actor.EnsureCanAssign();
        SupportTicket ticket = _tickets.GetRequired(ticketId);

        List<AgentWorkload> candidates = new();
        foreach (Agent agent in GetAgents())
        {
            int activeCount = CountActiveTickets(agent.Id, ticket.Id);
            if (agent.CanTake(activeCount, _settings.MaxActiveTickets))
            {
                candidates.Add(new AgentWorkload(agent, activeCount));
            }
        }

        Agent? chosen = _strategy.Pick(ticket.HandlingDepartmentCode, candidates);
        int chosenCount = chosen is null ? 0 : CountActiveTickets(chosen.Id, ticket.Id);
        ticket.AssignTo(chosen, chosenCount, _settings.MaxActiveTickets);
        _tickets.Save(ticket);

        // AssignTo succeeded, so the ticket now has an agent.
        return ticket.AssignedAgent!;
    }

    /// <summary>
    /// UC3: assigns or reassigns the ticket to the agent chosen by the manager.
    /// Steps: <see cref="Employee.EnsureCanAssign"/> (rule 11); find the agent; count their active tickets;
    /// <see cref="SupportTicket.AssignTo"/> (rules 1 and 3); save.
    /// </summary>
    /// <exception cref="BusinessRuleException">
    /// The actor is not a manager, the ticket or agent does not exist, the ticket is Resolved or Closed,
    /// or the agent already holds <see cref="BusinessSettings.MaxActiveTickets"/> active tickets.
    /// </exception>
    public void AssignManually(Employee actor, int ticketId, int agentId)
    {
        actor.EnsureCanAssign();
        SupportTicket ticket = _tickets.GetRequired(ticketId);
        Agent? agent = _employees.GetRequired(agentId) as Agent;
        int activeCount = agent is null ? 0 : CountActiveTickets(agent.Id, ticket.Id);

        ticket.AssignTo(agent, activeCount, _settings.MaxActiveTickets);
        _tickets.Save(ticket);
    }

    /// <summary>
    /// UC4: Open to InProgress. Steps: <see cref="SupportTicket.EnsureAssignedTo"/> (rule 11);
    /// <see cref="SupportTicket.Start"/> (rule 1); save.
    /// </summary>
    /// <exception cref="BusinessRuleException">The actor is not the assigned agent, or the ticket is not Open.</exception>
    public void Start(Employee actor, int ticketId, DateTime now)
    {
        SupportTicket ticket = _tickets.GetRequired(ticketId);
        ticket.EnsureAssignedTo(actor);
        ticket.Start(now);
        _tickets.Save(ticket);
    }

    /// <summary>
    /// UC4: InProgress to Resolved. Steps: <see cref="SupportTicket.EnsureAssignedTo"/> (rule 11);
    /// <see cref="SupportTicket.Resolve"/> (rule 1); save.
    /// </summary>
    /// <exception cref="ValidationException">The note is empty.</exception>
    /// <exception cref="BusinessRuleException">The actor is not the assigned agent, or the ticket is not InProgress.</exception>
    public void Resolve(Employee actor, int ticketId, string note, DateTime now)
    {
        SupportTicket ticket = _tickets.GetRequired(ticketId);
        ticket.EnsureAssignedTo(actor);
        ticket.Resolve(note, now);
        _tickets.Save(ticket);
    }

    /// <summary>
    /// UC4: Resolved to Closed. Steps: <see cref="Employee.EnsureCanClose"/> (rule 11);
    /// <see cref="SupportTicket.Close"/> (rule 1); save.
    /// </summary>
    /// <exception cref="BusinessRuleException">The actor is not a manager, or the ticket is not Resolved.</exception>
    public void Close(Employee actor, int ticketId, DateTime now)
    {
        actor.EnsureCanClose();
        SupportTicket ticket = _tickets.GetRequired(ticketId);
        ticket.Close(now);
        _tickets.Save(ticket);
    }

    /// <summary>
    /// UC4: Resolved back to InProgress. Steps: <see cref="Employee.EnsureCanReopen"/> (rule 11);
    /// <see cref="SupportTicket.Reopen"/> with <see cref="BusinessSettings.ReopenDays"/> (rules 1 and 2); save.
    /// </summary>
    /// <exception cref="BusinessRuleException">The actor is not a manager, the ticket is not Resolved, or the reopen window has passed.</exception>
    public void Reopen(Employee actor, int ticketId, DateTime now)
    {
        actor.EnsureCanReopen();
        SupportTicket ticket = _tickets.GetRequired(ticketId);
        ticket.Reopen(now, _settings.ReopenDays);
        _tickets.Save(ticket);
    }

    /// <summary>
    /// UC5: returns the tickets that match every given criterion, newest first.
    /// A null or empty criterion is not applied.
    /// </summary>
    /// <param name="stateName">State name such as <c>Open</c>.</param>
    /// <param name="priority">Stored (boosted) priority.</param>
    /// <param name="typeName">Ticket type name such as <c>Complaint</c>.</param>
    /// <param name="agentId">Id of the assigned agent.</param>
    /// <param name="keyword">Text contained in the title (case-insensitive).</param>
    public IReadOnlyList<SupportTicket> Search(
        string? stateName,
        TicketPriority? priority,
        string? typeName,
        int? agentId,
        string? keyword)
    {
        IEnumerable<SupportTicket> result = _tickets.GetAll();

        if (!string.IsNullOrEmpty(stateName))
        {
            result = result.Where(t => t.State.Name == stateName);
        }

        if (priority is not null)
        {
            result = result.Where(t => t.Priority == priority.Value);
        }

        if (!string.IsNullOrEmpty(typeName))
        {
            result = result.Where(t => t.TypeName == typeName);
        }

        if (agentId is not null)
        {
            result = result.Where(t => t.AssignedAgent?.Id == agentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string text = keyword.Trim();
            result = result.Where(t => t.Title.Contains(text, StringComparison.OrdinalIgnoreCase));
        }

        return result.OrderByDescending(t => t.CreatedAt).ToList();
    }

    /// <summary>Returns one ticket for the detail form.</summary>
    /// <exception cref="BusinessRuleException">The ticket does not exist.</exception>
    public SupportTicket GetById(int ticketId)
    {
        return _tickets.GetRequired(ticketId);
    }

    /// <summary>Returns the ticket type names that have a factory, for the type list of the forms.</summary>
    public IReadOnlyList<string> GetTicketTypes()
    {
        return _factories.Keys.ToList();
    }

    /// <summary>Returns every agent, for the manual assignment list and the search filter.</summary>
    public IReadOnlyList<Agent> GetAgents()
    {
        return _employees.GetAll().OfType<Agent>().ToList();
    }

    /// <summary>Finds customers for the ticket form; forwards to <see cref="ICustomerDirectory.Search"/>.</summary>
    public IReadOnlyList<CustomerSummary> FindCustomers(string keyword)
    {
        return _customers.Search(keyword);
    }

    /// <summary>Returns the summary of a ticket's customer; forwards to <see cref="ICustomerDirectory.GetSummary"/>.</summary>
    /// <exception cref="BusinessRuleException">The customer does not exist.</exception>
    public CustomerSummary GetCustomer(int customerId)
    {
        return _customers.GetSummary(customerId);
    }

    /// <summary>
    /// Returns the rooms the customer is staying in (room Id to room number), for the room list of a
    /// maintenance ticket; forwards to <see cref="IStayLookup.GetActiveRooms"/>.
    /// </summary>
    public IReadOnlyDictionary<int, string> GetActiveRooms(int customerId)
    {
        return _stays.GetActiveRooms(customerId);
    }

    /// <summary>Returns the number of a room; forwards to <see cref="IStayLookup.GetRoomNumber"/>.</summary>
    public string GetRoomNumber(int roomId)
    {
        return _stays.GetRoomNumber(roomId);
    }

    /// <summary>
    /// Rule 3: number of Open or InProgress tickets assigned to the agent, not counting the ticket
    /// being assigned (so reassigning a ticket to its current agent does not count it twice).
    /// </summary>
    private int CountActiveTickets(int agentId, int excludedTicketId)
    {
        return _tickets.GetAll().Count(t =>
            t.Id != excludedTicketId
            && t.AssignedAgent?.Id == agentId
            && t.State.IsActive);
    }

    private void AttachObservers(SupportTicket ticket)
    {
        foreach (ITicketObserver observer in _observers)
        {
            ticket.Attach(observer);
        }
    }
}
