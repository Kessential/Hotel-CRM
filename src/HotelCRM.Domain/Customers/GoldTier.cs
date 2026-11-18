namespace HotelCRM.Domain.Customers;

/// <summary>Third tier, reached at 5000 points. Tickets are handled 25% faster and start one priority level higher.</summary>
public sealed class GoldTier : MembershipTier
{
    /// <summary>Value of <see cref="Name"/> for this tier.</summary>
    public const string TierName = "Gold";

    /// <inheritdoc />
    public override string Name => TierName;

    /// <summary>0.75.</summary>
    public override double SlaFactor => 0.75;

    /// <summary>1.</summary>
    public override int PriorityBoost => 1;

    /// <summary>5000.</summary>
    public override int MinPoints => 5000;

    /// <summary>Returns <see cref="VipTier"/>.</summary>
    public override MembershipTier? Next()
    {
        return new VipTier();
    }
}
