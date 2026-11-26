namespace HotelCRM.Business.Segments;

/// <summary>Customers whose birthday falls in the month the campaign starts.</summary>
public sealed class BirthdayMonthSegment : ISegmentStrategy
{
    /// <inheritdoc />
    public string Code => "BIRTHDAY";

    /// <inheritdoc />
    public string DisplayName => "Sinh nhật trong tháng bắt đầu chiến dịch";

    /// <summary>
    /// True when the month of birth equals the month of <paramref name="referenceDate"/>.
    /// A customer without a date of birth is not included.
    /// </summary>
    public bool Includes(SegmentCandidate customer, DateTime referenceDate)
    {
        return customer.DateOfBirth is { } dateOfBirth && dateOfBirth.Month == referenceDate.Month;
    }
}
