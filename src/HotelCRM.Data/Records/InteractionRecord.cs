using HotelCRM.Domain.Customers;

namespace HotelCRM.Data.Records;

/// <summary>One entry of the <c>Interactions</c> array nested inside a customer in <c>customers.json</c>.</summary>
public sealed class InteractionRecord
{
    public DateTime OccurredAt { get; set; }

    public ContactChannel Channel { get; set; }

    public string Summary { get; set; } = string.Empty;

    public int? TicketId { get; set; }
}
