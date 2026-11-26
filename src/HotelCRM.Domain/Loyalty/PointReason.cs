namespace HotelCRM.Domain.Loyalty;

/// <summary>Why points were added to or taken from a customer's point ledger.</summary>
public enum PointReason
{
    /// <summary>Q9: points earned at check-out. Linked to the booking.</summary>
    Stay,

    /// <summary>Q12: goodwill points when a ticket of the customer is closed. Linked to the ticket.</summary>
    Goodwill,

    /// <summary>Q10: points spent on a reward (negative), or given back when the redemption is cancelled (positive).</summary>
    Redemption,
}
