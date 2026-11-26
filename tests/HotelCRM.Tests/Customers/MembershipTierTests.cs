using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Customers;

/// <summary>Q9 (points earned per tier) and Q10 (comparing a tier with the minimum tier of a reward).</summary>
public class MembershipTierTests
{
    /// <summary>
    /// Expected values worked out by hand: amount / 10,000 x earn rate, rounded down.
    /// 19,999 and 20,000 sit on both sides of a whole point; 25,000 x 1.2 = 3.0 exactly.
    /// </summary>
    [Theory]
    [InlineData(StandardTier.TierName, 19_999, 1)]
    [InlineData(StandardTier.TierName, 20_000, 2)]
    [InlineData(SilverTier.TierName, 25_000, 3)]
    [InlineData(SilverTier.TierName, 1_000_000, 120)]
    [InlineData(GoldTier.TierName, 1_000_000, 150)]
    [InlineData(VipTier.TierName, 1_500_000, 300)]
    public void PointsFor_RoomCharges_AppliesEarnRateAndRoundsDown(string tierName, int amountVnd, int expected)
    {
        // Arrange
        MembershipTier tier = MembershipTier.FromName(tierName);

        // Act
        int points = tier.PointsFor(amountVnd);

        // Assert
        Assert.Equal(expected, points);
    }

    [Fact]
    public void PointsFor_NegativeAmount_ThrowsValidation()
    {
        // Arrange
        MembershipTier tier = new StandardTier();

        // Act + Assert
        Assert.Throws<ValidationException>(() => tier.PointsFor(-1));
    }

    [Theory]
    [InlineData(GoldTier.TierName, SilverTier.TierName, true)]
    [InlineData(GoldTier.TierName, GoldTier.TierName, true)]
    [InlineData(SilverTier.TierName, GoldTier.TierName, false)]
    [InlineData(StandardTier.TierName, VipTier.TierName, false)]
    public void IsAtLeast_ComparedWithMinimumTier_FollowsTierOrder(string tierName, string minimumTierName, bool expected)
    {
        // Arrange
        MembershipTier tier = MembershipTier.FromName(tierName);
        MembershipTier minimum = MembershipTier.FromName(minimumTierName);

        // Act
        bool result = tier.IsAtLeast(minimum);

        // Assert
        Assert.Equal(expected, result);
    }
}
