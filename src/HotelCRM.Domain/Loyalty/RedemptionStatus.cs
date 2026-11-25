namespace HotelCRM.Domain.Loyalty;

/// <summary>Status of a reward redemption.</summary>
public enum RedemptionStatus
{
    /// <summary>The reward was handed over and the points were spent.</summary>
    Completed,

    /// <summary>The redemption was cancelled: the reward went back to stock and the points were refunded.</summary>
    Cancelled,
}
