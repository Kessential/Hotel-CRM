namespace HotelCRM.Data.Records;

/// <summary>
/// One entry of <c>customers.json</c>. <see cref="Tier"/> holds the tier name (<c>Standard</c>,
/// <c>Silver</c>, <c>Gold</c>, <c>VIP</c>); the interaction history is nested because it belongs to the customer.
/// </summary>
public sealed class CustomerRecord
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string IdentityNumber { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public string Tier { get; set; } = string.Empty;

    public int Points { get; set; }

    public List<InteractionRecord> Interactions { get; set; } = new();
}
