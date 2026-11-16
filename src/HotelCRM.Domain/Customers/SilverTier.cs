namespace HotelCRM.Domain.Customers;

/// <summary>Second tier, reached at 1000 points. Tickets are handled 10% faster.</summary>
public sealed class SilverTier : MembershipTier
{
    /// <summary>Value of <see cref="Name"/> for this tier.</summary>
    public const string TierName = "Silver";

    /// <inheritdoc />
    public override string Name => TierName;

    /// <summary>0.9.</summary>
    public override double SlaFactor => throw new NotImplementedException();

    /// <summary>0.</summary>
    public override int PriorityBoost => throw new NotImplementedException();

    /// <summary>1000.</summary>
    public override int MinPoints => throw new NotImplementedException();

    /// <summary>Returns <see cref="GoldTier"/>.</summary>
    public override MembershipTier? Next()
    {
        throw new NotImplementedException();
    }
}
