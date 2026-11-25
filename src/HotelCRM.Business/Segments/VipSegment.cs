using HotelCRM.Domain.Customers;

namespace HotelCRM.Business.Segments;

/// <summary>Customers in the highest membership tier.</summary>
public sealed class VipSegment : ISegmentStrategy
{
    private static readonly MembershipTier Vip = new VipTier();

    /// <inheritdoc />
    public string Code => "VIP";

    /// <inheritdoc />
    public string DisplayName => "Khách hạng VIP";

    /// <summary>True when the customer's tier is VIP or higher. The date is not used.</summary>
    public bool Includes(SegmentCandidate customer, DateTime referenceDate)
    {
        return customer.Tier.IsAtLeast(Vip);
    }
}
