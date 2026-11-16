using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Factories;

/// <summary>
/// Creates <see cref="MaintenanceTicket"/> objects, passing the room Id on to the ticket.
/// Registered under <see cref="MaintenanceTicket.TypeKey"/>.
/// </summary>
public sealed class MaintenanceTicketFactory : TicketFactory
{
    /// <inheritdoc />
    public override SupportTicket Create(
        int id,
        int customerId,
        string title,
        string description,
        TicketPriority priority,
        CustomerSlaProfile slaProfile,
        DateTime createdAt,
        int? roomId)
    {
        throw new NotImplementedException();
    }
}
