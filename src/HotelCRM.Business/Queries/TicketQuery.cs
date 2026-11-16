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
        throw new NotImplementedException();
    }

    /// <summary>True when the ticket exists and its state's <c>IsActive</c> is false.</summary>
    public bool IsResolvedOrClosed(int ticketId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<SupportTicket> GetByCustomer(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyList<SupportTicket> GetAll()
    {
        throw new NotImplementedException();
    }
}
