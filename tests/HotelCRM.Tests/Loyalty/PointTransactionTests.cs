using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Loyalty;

namespace HotelCRM.Tests.Loyalty;

/// <summary>Ledger lines: a redemption takes its fixed points, a cancelled one gives the same points back.</summary>
public class PointTransactionTests
{
    private static readonly DateTime Now = new(2026, 11, 20, 10, 0, 0);

    [Fact]
    public void ForRedemptionAndRefund_CancelledRedemption_CancelEachOut()
    {
        // Arrange
        Reward reward = new(1, "Voucher spa", 800, new StandardTier(), 3, true);
        RewardRedemption redemption = reward.Redeem(5, 7, new StandardTier(), 1_000, 3, Now);

        // Act
        PointTransaction spent = PointTransaction.ForRedemption(10, redemption);
        reward.CancelRedemption(redemption);
        PointTransaction refund = PointTransaction.ForRefund(11, redemption, Now.AddHours(1));

        // Assert
        Assert.Equal(-800, spent.Points);
        Assert.Equal(800, refund.Points);
        Assert.Equal(5, refund.RedemptionId);
    }

    [Fact]
    public void ForRefund_RedemptionNotCancelled_ThrowsBusinessRule()
    {
        // Arrange
        Reward reward = new(1, "Voucher spa", 800, new StandardTier(), 3, true);
        RewardRedemption redemption = reward.Redeem(5, 7, new StandardTier(), 1_000, 3, Now);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => PointTransaction.ForRefund(11, redemption, Now));
    }

    [Fact]
    public void ForRedemption_CancelledRedemption_ThrowsBusinessRule()
    {
        // Arrange
        Reward reward = new(1, "Voucher spa", 800, new StandardTier(), 3, true);
        RewardRedemption redemption = reward.Redeem(5, 7, new StandardTier(), 1_000, 3, Now);
        reward.CancelRedemption(redemption);

        // Act + Assert: a cancelled redemption must not take points.
        Assert.Throws<BusinessRuleException>(() => PointTransaction.ForRedemption(10, redemption));
    }

    [Fact]
    public void ForStay_ZeroPoints_ThrowsValidation()
    {
        // Act + Assert
        Assert.Throws<ValidationException>(() => PointTransaction.ForStay(1, 7, 0, 20, Now));
    }
}
