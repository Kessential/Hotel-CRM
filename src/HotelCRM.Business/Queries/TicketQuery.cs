using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Queries;

/// <summary>
/// Answers the customer subsystem's questions about tickets. Built on the ticket repository only
/// (not on <c>TicketService</c>), so the two subsystems' services never depend on each other.
/// </summary>
public sealed class TicketQuery : ITicketQuery
{
    private readonly IRepository<SupportTicket> _tickets;

    /// <summary>Creates the query object.</summary>
    /// <param name="tickets">Ticket repository.</param>
    public TicketQuery(IRepository<SupportTicket> tickets)
    {
        _tickets = tickets;
    }

    /// <summary>True when a ticket of the customer has a state whose <c>IsClosed</c> is false.</summary>
    public bool HasOpenTickets(int customerId)
    {
        return _tickets.GetAll().Any(t => t.CustomerId == customerId && !t.State.IsClosed);
    }

    /// <summary>True when the ticket exists and its state's <c>IsActive</c> is false.</summary>
    public bool IsResolvedOrClosed(int ticketId)
    {
        SupportTicket? ticket = _tickets.GetById(ticketId);
        return ticket is not null && !ticket.State.IsActive;
    }

    /// <inheritdoc />
    public IReadOnlyList<SupportTicket> GetByCustomer(int customerId)
    {
        return _tickets.GetAll()
            .Where(t => t.CustomerId == customerId)
            .OrderBy(t => t.CreatedAt)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyList<SupportTicket> GetAll()
    {
        return _tickets.GetAll();
    }
}
