namespace HotelCRM.Domain.Customers;

/// <summary>Tier of every new customer. No benefits.</summary>
public sealed class StandardTier : MembershipTier
{
    /// <summary>Value of <see cref="Name"/> for this tier.</summary>
    public const string TierName = "Standard";

    /// <inheritdoc />
    public override string Name => TierName;

    /// <summary>1.0.</summary>
    public override double SlaFactor => 1.0;

    /// <summary>0.</summary>
    public override int PriorityBoost => 0;

    /// <summary>1.0.</summary>
    public override decimal EarnRate => 1.0m;

    /// <summary>0.</summary>
    public override int MinPoints => 0;

    /// <summary>Returns <see cref="SilverTier"/>.</summary>
    public override MembershipTier? Next()
    {
        return new SilverTier();
    }
}
