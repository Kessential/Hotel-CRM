using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// Membership tier of a customer. Abstract: each tier is a subclass that supplies its own numbers,
/// so the code never switches on the tier. The numbers are constants of the classes, not configuration.
/// </summary>
public abstract class MembershipTier
{
    /// <summary>Q9: room charges, in VND, that earn one point before the tier's earn rate is applied.</summary>
    public const decimal VndPerPoint = 10_000m;

    /// <summary>Name of the tier: <c>Standard</c>, <c>Silver</c>, <c>Gold</c> or <c>VIP</c>. Stored in <c>customers.Tier</c>.</summary>
    public abstract string Name { get; }

    /// <summary>Q3: multiplier of the ticket handling time. Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5.</summary>
    public abstract double SlaFactor { get; }

    /// <summary>Q3: priority levels added to a new ticket. Standard 0, Silver 0, Gold 1, VIP 2.</summary>
    public abstract int PriorityBoost { get; }

    /// <summary>Q9: lifetime points needed to reach this tier. Standard 0, Silver 1000, Gold 5000, VIP 15000.</summary>
    public abstract int MinPoints { get; }

    /// <summary>Q9: multiplier applied to the points of a stay. Standard 1.0, Silver 1.2, Gold 1.5, VIP 2.0.</summary>
    public abstract decimal EarnRate { get; }

    /// <summary>
    /// Q9: points earned for <paramref name="amountVnd"/> of room charges:
    /// amount / <see cref="VndPerPoint"/> x <see cref="EarnRate"/>, rounded down.
    /// </summary>
    /// <exception cref="ValidationException">The amount is negative.</exception>
    public int PointsFor(decimal amountVnd)
    {
        if (amountVnd < 0)
        {
            throw new ValidationException("Số tiền tính điểm không được âm.");
        }

        return (int)decimal.Floor(amountVnd / VndPerPoint * EarnRate);
    }

    /// <summary>
    /// Q10: whether this tier is the same as or higher than <paramref name="other"/>, for example the
    /// minimum tier of a reward. Tiers are ordered by <see cref="MinPoints"/>, so a new tier needs no change here.
    /// </summary>
    public bool IsAtLeast(MembershipTier other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return MinPoints >= other.MinPoints;
    }

    /// <summary>
    /// Returns the tier object for a stored name by walking the ladder from <see cref="StandardTier"/>
    /// through <see cref="Next"/>. A new tier only has to be linked into the ladder; nothing changes here.
    /// </summary>
    /// <exception cref="ArgumentException">The name is not a known tier (corrupt stored data, not a business error).</exception>
    public static MembershipTier FromName(string name)
    {
        for (MembershipTier? tier = new StandardTier(); tier is not null; tier = tier.Next())
        {
            if (tier.Name == name)
            {
                return tier;
            }
        }

        throw new ArgumentException($"Hạng thành viên \"{name}\" không hợp lệ.", nameof(name));
    }

    /// <summary>Q9: returns the next higher tier, or null when this is the highest tier.</summary>
    public abstract MembershipTier? Next();

    /// <summary>Returns <see cref="Name"/>.</summary>
    public override string ToString()
    {
        return Name;
    }
}
