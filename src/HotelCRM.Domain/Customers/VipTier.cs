namespace HotelCRM.Domain.Customers;

/// <summary>Highest tier, reached at 15000 points. Tickets are handled twice as fast and start two priority levels higher.</summary>
public sealed class VipTier : MembershipTier
{
    /// <summary>Value of <see cref="Name"/> for this tier.</summary>
    public const string TierName = "VIP";

    /// <inheritdoc />
    public override string Name => TierName;

    /// <summary>0.5.</summary>
    public override double SlaFactor => throw new NotImplementedException();

    /// <summary>2.</summary>
    public override int PriorityBoost => throw new NotImplementedException();

    /// <summary>15000.</summary>
    public override int MinPoints => throw new NotImplementedException();

    /// <summary>Returns null: there is no tier above VIP.</summary>
    public override MembershipTier? Next()
    {
        throw new NotImplementedException();
    }
}
