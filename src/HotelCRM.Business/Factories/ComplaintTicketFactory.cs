using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Factories;

/// <summary>Creates <see cref="ComplaintTicket"/> objects. Registered under <see cref="ComplaintTicket.TypeKey"/>.</summary>
public sealed class ComplaintTicketFactory : TicketFactory
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
