using HotelCRM.Domain.Tickets;

namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// Boundary interface: what the customer subsystem (share B) needs from the ticket subsystem (share A).
/// Read-only. Implemented by <c>TicketQuery</c> in <c>Business/Queries</c>; tests use <c>FakeTicketQuery</c>.
/// </summary>
public interface ITicketQuery
{
    /// <summary>Rule 10: whether the customer has at least one ticket that is not Closed.</summary>
    bool HasOpenTickets(int customerId);

    /// <summary>Rule 9: whether the ticket exists and is Resolved or Closed.</summary>
    bool IsResolvedOrClosed(int ticketId);

    /// <summary>Returns every ticket of one customer, oldest first.</summary>
    IReadOnlyList<SupportTicket> GetByCustomer(int customerId);

    /// <summary>Returns every ticket; used by the reports.</summary>
    IReadOnlyList<SupportTicket> GetAll();
}
