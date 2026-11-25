namespace HotelCRM.Business.Segments;

/// <summary>
/// Customers who stayed before but have not come back for a while. The number of days comes from
/// <c>appsettings.json</c> and is passed in by <c>Program.cs</c>.
/// </summary>
public sealed class LapsedGuestSegment : ISegmentStrategy
{
    private readonly int _lapsedDays;

    /// <summary>Creates the segment.</summary>
    /// <param name="lapsedDays">Days since the last check-out from which a customer counts as lapsed.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="lapsedDays"/> is not positive (configuration error).</exception>
    public LapsedGuestSegment(int lapsedDays)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lapsedDays);
        _lapsedDays = lapsedDays;
    }

    /// <inheritdoc />
    public string Code => "LAPSED";

    /// <inheritdoc />
    public string DisplayName => $"Khách lâu chưa quay lại (từ {_lapsedDays} ngày)";

    /// <summary>
    /// True when the customer has stayed at least once and the last check-out is <c>lapsedDays</c> or more
    /// days before <paramref name="referenceDate"/>. A customer who never stayed is not lapsed.
    /// </summary>
    public bool Includes(SegmentCandidate customer, DateTime referenceDate)
    {
        if (customer.LastCheckOut is not { } lastCheckOut)
        {
            return false;
        }

        return (referenceDate.Date - lastCheckOut.Date).Days >= _lapsedDays;
    }
}
