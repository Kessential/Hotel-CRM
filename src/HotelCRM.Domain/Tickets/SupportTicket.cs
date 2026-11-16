using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Domain.Tickets;

/// <summary>
/// A customer care ticket. Abstract: the kind of ticket is a subclass, not an enum, and each subclass
/// overrides <see cref="BaseSlaHours"/>, <see cref="HandlingDepartmentCode"/>, <see cref="GoodwillPoints"/>
/// and <see cref="Validate"/>.
/// <para>
/// Context of the State pattern: <see cref="Start"/>, <see cref="Resolve"/>, <see cref="Close"/> and
/// <see cref="Reopen"/> delegate to the current <see cref="TicketState"/>, which decides the next state.
/// Subject of the Observer pattern: observers attached with <see cref="Attach"/> are notified after every
/// state change.
/// </para>
/// The ticket refers to its customer and room by Id only, so this subsystem does not depend on the
/// customer and hotel classes.
/// </summary>
public abstract class SupportTicket : IEntity
{
    private readonly List<ITicketObserver> _observers = new();

    /// <summary>
    /// Creates a new ticket in the Open state with no agent.
    /// Rule 4: the requested priority is raised by <see cref="CustomerSlaProfile.PriorityBoost"/> levels
    /// (never past Urgent) and stored; <see cref="DueAt"/> is then set to
    /// <see cref="CalculateDueAt"/> with <see cref="CustomerSlaProfile.SlaFactor"/>.
    /// </summary>
    /// <param name="id">Id obtained from the ticket repository.</param>
    /// <param name="customerId">Id of the customer the ticket belongs to.</param>
    /// <param name="title">Short title; required.</param>
    /// <param name="description">Details; may be empty unless the subclass requires it.</param>
    /// <param name="requestedPriority">Priority chosen by the employee, before the tier boost.</param>
    /// <param name="slaProfile">SLA numbers of the customer's membership tier.</param>
    /// <param name="createdAt">Creation time (the caller's "now").</param>
    /// <exception cref="ValidationException">The title is empty.</exception>
    protected SupportTicket(
        int id,
        int customerId,
        string title,
        string description,
        TicketPriority requestedPriority,
        CustomerSlaProfile slaProfile,
        DateTime createdAt)
    {
        Id = id;
        CustomerId = customerId;
        Title = title;
        Description = description;
        Priority = requestedPriority;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        State = new OpenState();
        ApplyCreationRules(slaProfile);
    }

    /// <summary>Rebuilds a ticket from stored values without running the creation rules.</summary>
    private protected SupportTicket(TicketSnapshot snapshot)
    {
        Id = snapshot.Id;
        CustomerId = snapshot.CustomerId;
        Title = snapshot.Title;
        Description = snapshot.Description;
        Priority = snapshot.Priority;
        CreatedAt = snapshot.CreatedAt;
        DueAt = snapshot.DueAt;
        UpdatedAt = snapshot.UpdatedAt;
        State = snapshot.State;
        AssignedAgent = snapshot.AssignedAgent;
        ResolvedAt = snapshot.ResolvedAt;
        ResolutionNote = snapshot.ResolutionNote;
        ClosedAt = snapshot.ClosedAt;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Id of the customer the ticket belongs to.</summary>
    public int CustomerId { get; }

    /// <summary>Short title; never empty.</summary>
    public string Title { get; }

    /// <summary>Details of the request.</summary>
    public string Description { get; }

    /// <summary>Priority after the tier boost of rule 4; fixed at creation.</summary>
    public TicketPriority Priority { get; private set; }

    /// <summary>Creation time.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>Deadline computed at creation by <see cref="CalculateDueAt"/>.</summary>
    public DateTime DueAt { get; private set; }

    /// <summary>Time of the last state change (the creation time until the first change).</summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>Current state; replaced only by the state classes.</summary>
    public TicketState State { get; private set; }

    /// <summary>Agent the ticket is assigned to, or null while unassigned. Kept when the ticket is reopened.</summary>
    public Agent? AssignedAgent { get; private set; }

    /// <summary>Time of the most recent resolution; null until the ticket is resolved for the first time. Kept on reopen.</summary>
    public DateTime? ResolvedAt { get; private set; }

    /// <summary>Note written by the agent at the most recent resolution.</summary>
    public string? ResolutionNote { get; private set; }

    /// <summary>Time the ticket was closed; null until then.</summary>
    public DateTime? ClosedAt { get; private set; }

    /// <summary>
    /// Name of the ticket kind: <c>Complaint</c>, <c>ServiceRequest</c> or <c>Maintenance</c>.
    /// Used as the key of the factory table, as the <c>Type</c> field in <c>tickets.json</c> and as a search criterion.
    /// </summary>
    public abstract string TypeName { get; }

    /// <summary>Handling time in hours before the multipliers: Complaint 24, ServiceRequest 8, Maintenance 12.</summary>
    public abstract int BaseSlaHours { get; }

    /// <summary>
    /// Code of the department that handles this kind of ticket:
    /// Complaint <c>GUEST</c>, ServiceRequest <c>HOUSE</c>, Maintenance <c>ENG</c>.
    /// </summary>
    public abstract string HandlingDepartmentCode { get; }

    /// <summary>Loyalty points the customer receives when the ticket is closed: 10 by default, 50 for a complaint.</summary>
    public virtual int GoodwillPoints => throw new NotImplementedException();

    /// <summary>
    /// Hours between creation and the most recent resolution (<c>ResolvedAt - CreatedAt</c>);
    /// null when the ticket has never been resolved.
    /// </summary>
    public double? ResolutionHours => throw new NotImplementedException();

    /// <summary>
    /// Rule 4: returns <c>CreatedAt + BaseSlaHours x priority multiplier x slaFactor</c> hours, using the
    /// stored (already boosted) priority. Priority multipliers: Low 2.0, Medium 1.0, High 0.5, Urgent 0.25.
    /// Example: Complaint, High, factor 0.75 gives 24 x 0.5 x 0.75 = 9 hours after creation.
    /// </summary>
    /// <param name="slaFactor">Factor of the customer's tier: Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5.</param>
    public DateTime CalculateDueAt(double slaFactor)
    {
        throw new NotImplementedException();
    }

    /// <summary>Whether the ticket missed its deadline: <c>(ResolvedAt ?? now) &gt; DueAt</c>.</summary>
    public bool IsOverdue(DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks the rules of the ticket kind that need looked-up data. The service calls it right after
    /// the factory created the ticket and before saving. The base version accepts everything;
    /// <see cref="ComplaintTicket"/> and <see cref="MaintenanceTicket"/> override it.
    /// </summary>
    /// <param name="roomInActiveStay">
    /// Result of <see cref="IStayLookup.IsRoomInActiveStay"/> for the ticket's customer and room;
    /// false when no room was given.
    /// </param>
    public virtual void Validate(bool roomInActiveStay)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assigns or reassigns the ticket (rules 1 and 3). Allowed only while the ticket is Open or InProgress.
    /// Calls <see cref="Agent.EnsureCanTake"/> with the two numbers. The state does not change.
    /// </summary>
    /// <param name="agent">Agent to assign; null when no agent is available (automatic assignment found nobody).</param>
    /// <param name="activeCount">Number of Open or InProgress tickets the agent already holds, counted by the service.</param>
    /// <param name="maxActive"><see cref="BusinessSettings.MaxActiveTickets"/>.</param>
    /// <exception cref="BusinessRuleException">
    /// <paramref name="agent"/> is null, the ticket is Resolved or Closed, or the agent already holds
    /// <paramref name="maxActive"/> active tickets.
    /// </exception>
    public void AssignTo(Agent? agent, int activeCount, int maxActive)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 11: stops the operation unless the acting employee is the agent this ticket is assigned to.</summary>
    /// <exception cref="BusinessRuleException">The ticket is unassigned or assigned to somebody else.</exception>
    public void EnsureAssignedTo(Employee actor)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 1: Open to InProgress. Delegates to the current state.</summary>
    /// <param name="now">Time of the change.</param>
    /// <exception cref="BusinessRuleException">The ticket is not Open, or no agent is assigned.</exception>
    public void Start(DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 1: InProgress to Resolved. Stores the note and the resolution time. Delegates to the current state.</summary>
    /// <param name="note">What was done; required.</param>
    /// <param name="now">Time of the change.</param>
    /// <exception cref="ValidationException">The note is empty.</exception>
    /// <exception cref="BusinessRuleException">The ticket is not InProgress.</exception>
    public void Resolve(string note, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 1: Resolved to Closed. A closed ticket never changes again. Delegates to the current state.</summary>
    /// <param name="now">Time of the change.</param>
    /// <exception cref="BusinessRuleException">The ticket is not Resolved.</exception>
    public void Close(DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Rules 1 and 2: Resolved back to InProgress, keeping the agent, allowed only while
    /// <c>now &lt;= ResolvedAt + reopenDays</c>. Delegates to the current state.
    /// </summary>
    /// <param name="now">Time of the change.</param>
    /// <param name="reopenDays"><see cref="BusinessSettings.ReopenDays"/>.</param>
    /// <exception cref="BusinessRuleException">The ticket is not Resolved, or the reopen window has passed.</exception>
    public void Reopen(DateTime now, int reopenDays)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Registers an observer that is notified after every state change. Attaching the same observer
    /// twice has no effect. Observers are not stored in the data file: the ticket service attaches
    /// them again whenever tickets are loaded or created.
    /// </summary>
    public void Attach(ITicketObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    /// <summary>
    /// For the state classes: replaces the current state, sets <see cref="UpdatedAt"/> and then
    /// notifies every observer with the old and the new state.
    /// </summary>
    internal void MoveTo(TicketState next, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>For the state classes: stores the resolution note and time.</summary>
    internal void RecordResolution(string note, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>For the state classes: stores the closing time.</summary>
    internal void RecordClosure(DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creation rules: the title is required; rule 4 raises <see cref="Priority"/> and sets <see cref="DueAt"/>.
    /// </summary>
    private void ApplyCreationRules(CustomerSlaProfile slaProfile)
    {
        throw new NotImplementedException();
    }

    /// <summary>Calls <see cref="ITicketObserver.OnStateChanged"/> on every attached observer; exceptions propagate.</summary>
    private void Notify(TicketState from, TicketState to)
    {
        throw new NotImplementedException();
    }
}
