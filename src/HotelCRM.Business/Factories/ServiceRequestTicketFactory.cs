using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Factories;

/// <summary>Creates <see cref="ServiceRequestTicket"/> objects. Registered under <see cref="ServiceRequestTicket.TypeKey"/>.</summary>
public sealed class ServiceRequestTicketFactory : TicketFactory
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
        return new ServiceRequestTicket(id, customerId, title, description, priority, slaProfile, createdAt);
    }
}
