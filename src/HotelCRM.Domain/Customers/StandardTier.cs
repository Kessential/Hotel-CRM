namespace HotelCRM.Domain.Customers;

/// <summary>Tier of every new customer. No benefits.</summary>
public sealed class StandardTier : MembershipTier
{
    /// <summary>Value of <see cref="Name"/> for this tier.</summary>
    public const string TierName = "Standard";

    /// <inheritdoc />
    public override string Name => TierName;

    /// <summary>1.0.</summary>
    public override double SlaFactor => throw new NotImplementedException();

    /// <summary>0.</summary>
    public override int PriorityBoost => throw new NotImplementedException();

    /// <summary>0.</summary>
    public override int MinPoints => throw new NotImplementedException();

    /// <summary>Returns <see cref="SilverTier"/>.</summary>
    public override MembershipTier? Next()
    {
        throw new NotImplementedException();
    }
}
