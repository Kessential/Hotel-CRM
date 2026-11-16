using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Factories;

/// <summary>
/// Creator of the Factory Method pattern. Each subclass creates one kind of ticket.
/// <c>TicketService.CreateTicket</c> finds the factory in a lookup table (ticket type name to factory)
/// built in <c>Program.cs</c>, so it never names a concrete ticket class and never switches on the type.
/// </summary>
public abstract class TicketFactory
{
    /// <summary>Creates a new ticket of the kind this factory is responsible for.</summary>
    /// <param name="id">Id obtained from the ticket repository.</param>
    /// <param name="customerId">Id of the customer the ticket belongs to.</param>
    /// <param name="title">Short title; required.</param>
    /// <param name="description">Details of the request.</param>
    /// <param name="priority">Priority chosen by the employee, before the tier boost of rule 4.</param>
    /// <param name="slaProfile">SLA numbers of the customer's tier, from <see cref="ICustomerDirectory.GetSlaProfile"/>.</param>
    /// <param name="createdAt">Creation time (the caller's "now").</param>
    /// <param name="roomId">Room the ticket is about; required by maintenance tickets, ignored by the other kinds.</param>
    /// <returns>The new ticket: Open, unassigned, with boosted priority and due time.</returns>
    /// <exception cref="ValidationException">Thrown by the ticket constructor: empty title, or missing room for a maintenance ticket.</exception>
    public abstract SupportTicket Create(
        int id,
        int customerId,
        string title,
        string description,
        TicketPriority priority,
        CustomerSlaProfile slaProfile,
        DateTime createdAt,
        int? roomId);
}
