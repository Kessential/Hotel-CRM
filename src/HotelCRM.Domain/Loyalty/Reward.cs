using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Loyalty;

/// <summary>
/// A gift in the reward catalogue. Decides Q10: whether a customer may redeem it, and keeps its stock
/// in step with the redemptions it creates and cancels.
/// </summary>
public sealed class Reward : IEntity
{
    /// <summary>Creates a reward and checks its data.</summary>
    /// <exception cref="ValidationException">Name is empty, cost is not positive, minimum tier is missing or quantity is negative.</exception>
    public Reward(int id, string name, int pointsCost, MembershipTier minTier, int quantity, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tên quà tặng không được để trống.");
        }

        if (pointsCost <= 0)
        {
            throw new ValidationException("Điểm quy đổi phải lớn hơn 0.");
        }

        if (quantity < 0)
        {
            throw new ValidationException("Số lượng quà không được âm.");
        }

        Id = id;
        Name = name.Trim();
        PointsCost = pointsCost;
        MinTier = minTier ?? throw new ValidationException("Phải chọn hạng tối thiểu của quà.");
        Quantity = quantity;
        IsActive = isActive;
    }

    /// <inheritdoc />
    public int Id { get; }

    /// <summary>Name of the gift.</summary>
    public string Name { get; }

    /// <summary>Points a customer spends for one unit.</summary>
    public int PointsCost { get; }

    /// <summary>Q10: lowest membership tier allowed to redeem this gift.</summary>
    public MembershipTier MinTier { get; }

    /// <summary>Units left in stock.</summary>
    public int Quantity { get; private set; }

    /// <summary>Whether the gift is still offered.</summary>
    public bool IsActive { get; }

    /// <summary>
    /// Q10: stops the redemption when the gift is no longer offered or out of stock, the customer's tier
    /// is below <see cref="MinTier"/>, or the point balance is below <see cref="PointsCost"/>.
    /// The service looks up the tier and balance; this method decides.
    /// </summary>
    /// <exception cref="BusinessRuleException">One of the conditions above is broken.</exception>
    public void EnsureCanRedeem(MembershipTier customerTier, int pointBalance)
    {
        if (!IsActive)
        {
            throw new BusinessRuleException($"Quà \"{Name}\" đã ngừng áp dụng.");
        }

        if (Quantity == 0)
        {
            throw new BusinessRuleException($"Quà \"{Name}\" đã hết.");
        }

        if (!customerTier.IsAtLeast(MinTier))
        {
            throw new BusinessRuleException(
                $"Quà \"{Name}\" dành cho hạng {MinTier.Name} trở lên; khách đang ở hạng {customerTier.Name}.");
        }

        if (pointBalance < PointsCost)
        {
            throw new BusinessRuleException(
                $"Quà \"{Name}\" cần {PointsCost} điểm; khách chỉ còn {pointBalance} điểm.");
        }
    }

    /// <summary>
    /// Q10: checks <see cref="EnsureCanRedeem"/>, takes one unit from stock and returns the redemption
    /// with <see cref="PointsCost"/> fixed as the points spent.
    /// </summary>
    /// <exception cref="BusinessRuleException">See <see cref="EnsureCanRedeem"/>.</exception>
    public RewardRedemption Redeem(
        int redemptionId,
        int customerId,
        MembershipTier customerTier,
        int pointBalance,
        int processedById,
        DateTime now)
    {
        EnsureCanRedeem(customerTier, pointBalance);
        Quantity--;
        return new RewardRedemption(redemptionId, customerId, Id, PointsCost, now, processedById);
    }

    /// <summary>Cancels a redemption of this gift and puts the unit back in stock.</summary>
    /// <exception cref="BusinessRuleException">The redemption belongs to another gift or is already cancelled.</exception>
    public void CancelRedemption(RewardRedemption redemption)
    {
        if (redemption.RewardId != Id)
        {
            throw new BusinessRuleException($"Lượt đổi quà {redemption.Id} không thuộc quà \"{Name}\".");
        }

        redemption.Cancel();
        Quantity++;
    }
}
