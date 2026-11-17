using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Tests.Fakes;

/// <summary>
/// Hand-written stand-in for the ticket subsystem. Lets the customer and survey tests run without
/// creating real tickets: a test just declares which customers have open tickets and which tickets
/// are resolved or closed.
/// </summary>
internal sealed class FakeTicketQuery : ITicketQuery
{
    private readonly HashSet<int> _customersWithOpenTickets = new();
    private readonly HashSet<int> _resolvedOrClosedTickets = new();

    /// <summary>Declares that the customer has a ticket that is not closed.</summary>
    public void AddOpenTicketFor(int customerId)
    {
        _customersWithOpenTickets.Add(customerId);
    }

    /// <summary>Declares that the ticket is Resolved or Closed.</summary>
    public void MarkResolvedOrClosed(int ticketId)
    {
        _resolvedOrClosedTickets.Add(ticketId);
    }

    public bool HasOpenTickets(int customerId)
    {
        return _customersWithOpenTickets.Contains(customerId);
    }

    public bool IsResolvedOrClosed(int ticketId)
    {
        return _resolvedOrClosedTickets.Contains(ticketId);
    }

    public IReadOnlyList<SupportTicket> GetByCustomer(int customerId)
    {
        return Array.Empty<SupportTicket>();
    }

    public IReadOnlyList<SupportTicket> GetAll()
    {
        return Array.Empty<SupportTicket>();
    }
}
