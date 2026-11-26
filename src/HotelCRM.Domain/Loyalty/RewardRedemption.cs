using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Loyalty;

/// <summary>
/// One reward handed to one customer (the n-n link between customers and rewards).
/// Only <see cref="Reward.Redeem"/> creates it and only <see cref="Reward.CancelRedemption"/> cancels it,
/// so the stock of the reward always matches its redemptions.
/// </summary>
public sealed class RewardRedemption : IEntity
{
    internal RewardRedemption(int id, int customerId, int rewardId, int pointsSpent, DateTime redeemedAt, int processedById)
    {
        Id = id;
        CustomerId = customerId;
        RewardId = rewardId;
        PointsSpent = pointsSpent;
        RedeemedAt = redeemedAt;
        ProcessedById = processedById;
        Status = RedemptionStatus.Completed;
    }

    /// <inheritdoc />
    public int Id { get; }

    /// <summary>Customer who received the reward.</summary>
    public int CustomerId { get; }

    /// <summary>Reward that was handed over.</summary>
    public int RewardId { get; }

    /// <summary>
    /// Q10: points cost of the reward at the moment of the redemption. Fixed here, so a later change
    /// of the reward's cost does not rewrite history.
    /// </summary>
    public int PointsSpent { get; }

    /// <summary>When the reward was handed over.</summary>
    public DateTime RedeemedAt { get; }

    /// <summary>Employee who processed the redemption.</summary>
    public int ProcessedById { get; }

    /// <summary>Completed or Cancelled.</summary>
    public RedemptionStatus Status { get; private set; }

    /// <summary>Marks the redemption as cancelled. Called by <see cref="Reward.CancelRedemption"/>.</summary>
    /// <exception cref="BusinessRuleException">The redemption is already cancelled.</exception>
    internal void Cancel()
    {
        if (Status == RedemptionStatus.Cancelled)
        {
            throw new BusinessRuleException($"Lượt đổi quà {Id} đã bị hủy trước đó.");
        }

        Status = RedemptionStatus.Cancelled;
    }
}
