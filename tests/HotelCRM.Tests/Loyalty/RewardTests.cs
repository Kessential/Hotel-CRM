using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Loyalty;

namespace HotelCRM.Tests.Loyalty;

/// <summary>Q10: redeeming a reward needs an active gift in stock, a high enough tier and enough points.</summary>
public class RewardTests
{
    private const int CustomerId = 7;
    private const int ReceptionistId = 3;
    private static readonly DateTime Now = new(2026, 11, 20, 10, 0, 0);

    [Fact]
    public void Redeem_BalanceEqualToCost_FixesPointsSpentAndTakesOneFromStock()
    {
        // Arrange: 500-point gift for Silver and above, 2 in stock; a Gold customer with exactly 500 points.
        Reward reward = NewReward(pointsCost: 500, minTier: new SilverTier(), quantity: 2);

        // Act
        RewardRedemption redemption = reward.Redeem(1, CustomerId, new GoldTier(), 500, ReceptionistId, Now);

        // Assert
        Assert.Equal(500, redemption.PointsSpent);
        Assert.Equal(RedemptionStatus.Completed, redemption.Status);
        Assert.Equal(1, reward.Quantity);
    }

    [Fact]
    public void Redeem_BalanceOneBelowCost_ThrowsBusinessRuleAndKeepsStock()
    {
        // Arrange
        Reward reward = NewReward(pointsCost: 500, minTier: new StandardTier(), quantity: 2);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => reward.Redeem(1, CustomerId, new StandardTier(), 499, ReceptionistId, Now));
        Assert.Equal(2, reward.Quantity);
    }

    [Fact]
    public void Redeem_TierBelowMinimum_ThrowsBusinessRule()
    {
        // Arrange: Gold-only gift, Silver customer with plenty of points.
        Reward reward = NewReward(pointsCost: 500, minTier: new GoldTier(), quantity: 2);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => reward.Redeem(1, CustomerId, new SilverTier(), 10_000, ReceptionistId, Now));
    }

    [Fact]
    public void Redeem_OutOfStock_ThrowsBusinessRule()
    {
        // Arrange
        Reward reward = NewReward(pointsCost: 500, minTier: new StandardTier(), quantity: 0);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => reward.Redeem(1, CustomerId, new VipTier(), 10_000, ReceptionistId, Now));
    }

    [Fact]
    public void Redeem_InactiveReward_ThrowsBusinessRule()
    {
        // Arrange
        Reward reward = NewReward(pointsCost: 500, minTier: new StandardTier(), quantity: 5, isActive: false);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => reward.Redeem(1, CustomerId, new VipTier(), 10_000, ReceptionistId, Now));
    }

    [Fact]
    public void CancelRedemption_SecondTime_ThrowsBusinessRuleAndReturnsStockOnce()
    {
        // Arrange: one redemption, then one cancellation (stock 2 -> 1 -> 2).
        Reward reward = NewReward(pointsCost: 500, minTier: new StandardTier(), quantity: 2);
        RewardRedemption redemption = reward.Redeem(1, CustomerId, new StandardTier(), 500, ReceptionistId, Now);
        reward.CancelRedemption(redemption);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => reward.CancelRedemption(redemption));
        Assert.Equal(2, reward.Quantity);
    }

    private static Reward NewReward(int pointsCost, MembershipTier minTier, int quantity, bool isActive = true)
    {
        return new Reward(1, "Bữa tối cho hai người", pointsCost, minTier, quantity, isActive);
    }
}
