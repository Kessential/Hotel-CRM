using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Loyalty;

/// <summary>
/// One line of a customer's point ledger: points added (positive) or spent (negative), with the reason
/// and the booking, ticket or redemption that caused it. Created only through the <c>For…</c> methods,
/// so a line always carries the link its reason needs.
/// </summary>
public sealed class PointTransaction : IEntity
{
    private PointTransaction(
        int id,
        int customerId,
        int points,
        PointReason reason,
        DateTime createdAt,
        int? bookingId,
        int? ticketId,
        int? redemptionId)
    {
        Id = id;
        CustomerId = customerId;
        Points = points;
        Reason = reason;
        CreatedAt = createdAt;
        BookingId = bookingId;
        TicketId = ticketId;
        RedemptionId = redemptionId;
    }

    /// <inheritdoc />
    public int Id { get; }

    /// <summary>Owner of the ledger.</summary>
    public int CustomerId { get; }

    /// <summary>Points added (positive) or spent (negative). Never 0.</summary>
    public int Points { get; }

    /// <summary>Why the points changed.</summary>
    public PointReason Reason { get; }

    /// <summary>When the line was written.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>Booking that earned the points; set only for <see cref="PointReason.Stay"/>.</summary>
    public int? BookingId { get; }

    /// <summary>Closed ticket behind goodwill points; set only for <see cref="PointReason.Goodwill"/>.</summary>
    public int? TicketId { get; }

    /// <summary>Redemption behind the line; set only for <see cref="PointReason.Redemption"/>.</summary>
    public int? RedemptionId { get; }

    /// <summary>Q9: points earned by a stay, written at check-out.</summary>
    /// <exception cref="ValidationException">Points are not positive.</exception>
    public static PointTransaction ForStay(int id, int customerId, int points, int bookingId, DateTime createdAt)
    {
        EnsurePositive(points);
        return new PointTransaction(id, customerId, points, PointReason.Stay, createdAt, bookingId, null, null);
    }

    /// <summary>Q12: goodwill points given when a ticket of the customer is closed.</summary>
    /// <exception cref="ValidationException">Points are not positive.</exception>
    public static PointTransaction ForGoodwill(int id, int customerId, int points, int ticketId, DateTime createdAt)
    {
        EnsurePositive(points);
        return new PointTransaction(id, customerId, points, PointReason.Goodwill, createdAt, null, ticketId, null);
    }

    /// <summary>Q10: takes the points fixed in <paramref name="redemption"/> from the ledger.</summary>
    /// <exception cref="BusinessRuleException">The redemption is cancelled, so there is nothing to spend.</exception>
    public static PointTransaction ForRedemption(int id, RewardRedemption redemption)
    {
        if (redemption.Status != RedemptionStatus.Completed)
        {
            throw new BusinessRuleException($"Lượt đổi quà {redemption.Id} đã bị hủy nên không trừ điểm.");
        }

        return new PointTransaction(
            id,
            redemption.CustomerId,
            -redemption.PointsSpent,
            PointReason.Redemption,
            redemption.RedeemedAt,
            null,
            null,
            redemption.Id);
    }

    /// <summary>Gives back the points of a cancelled redemption: the same amount it took, with a positive sign.</summary>
    /// <exception cref="BusinessRuleException">The redemption is not cancelled.</exception>
    public static PointTransaction ForRefund(int id, RewardRedemption redemption, DateTime createdAt)
    {
        if (redemption.Status != RedemptionStatus.Cancelled)
        {
            throw new BusinessRuleException($"Lượt đổi quà {redemption.Id} chưa bị hủy nên không hoàn điểm.");
        }

        return new PointTransaction(
            id,
            redemption.CustomerId,
            redemption.PointsSpent,
            PointReason.Redemption,
            createdAt,
            null,
            null,
            redemption.Id);
    }

    private static void EnsurePositive(int points)
    {
        if (points <= 0)
        {
            throw new ValidationException("Số điểm cộng phải lớn hơn 0.");
        }
    }
}
