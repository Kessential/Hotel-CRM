using HotelCRM.Business.Segments;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Segments;

/// <summary>Q11: the three segment strategies choose the right customers, checked on the date boundaries.</summary>
public class SegmentStrategyTests
{
    private const int LapsedDays = 180;
    private static readonly DateTime CampaignStart = new(2026, 12, 1);

    [Theory]
    [InlineData(VipTier.TierName, true)]
    [InlineData(GoldTier.TierName, false)]
    [InlineData(StandardTier.TierName, false)]
    public void VipSegment_ByTier_IncludesOnlyVip(string tierName, bool expected)
    {
        // Arrange
        ISegmentStrategy segment = new VipSegment();
        SegmentCandidate customer = Candidate(tier: MembershipTier.FromName(tierName));

        // Act
        bool included = segment.Includes(customer, CampaignStart);

        // Assert
        Assert.Equal(expected, included);
    }

    /// <summary>180 days before 01/12/2026 is 04/06/2026: exactly on the limit counts as lapsed.</summary>
    [Theory]
    [InlineData(180, true)]
    [InlineData(179, false)]
    [InlineData(400, true)]
    public void LapsedGuestSegment_DaysSinceLastCheckOut_IncludesFromLimit(int daysAgo, bool expected)
    {
        // Arrange: an older stay and the latest one; only the latest counts.
        ISegmentStrategy segment = new LapsedGuestSegment(LapsedDays);
        SegmentCandidate customer = Candidate(stays: [CampaignStart.AddDays(-500), CampaignStart.AddDays(-daysAgo)]);

        // Act
        bool included = segment.Includes(customer, CampaignStart);

        // Assert
        Assert.Equal(expected, included);
    }

    [Fact]
    public void LapsedGuestSegment_NeverStayed_NotIncluded()
    {
        // Arrange
        ISegmentStrategy segment = new LapsedGuestSegment(LapsedDays);

        // Act
        bool included = segment.Includes(Candidate(), CampaignStart);

        // Assert
        Assert.False(included);
    }

    /// <summary>Born on the first and last day of December, and on 30/11 just before the month starts.</summary>
    [Theory]
    [InlineData(1990, 12, 1, true)]
    [InlineData(1985, 12, 31, true)]
    [InlineData(1990, 11, 30, false)]
    public void BirthdayMonthSegment_ByDateOfBirth_IncludesCampaignMonth(int year, int month, int day, bool expected)
    {
        // Arrange
        ISegmentStrategy segment = new BirthdayMonthSegment();
        SegmentCandidate customer = Candidate(dateOfBirth: new DateTime(year, month, day));

        // Act
        bool included = segment.Includes(customer, CampaignStart);

        // Assert
        Assert.Equal(expected, included);
    }

    [Fact]
    public void Strategies_SameCustomers_EachChoosesItsOwnSegment()
    {
        // Arrange: customer 1 is VIP, 2 has not come back for a year, 3 was born in December.
        SegmentCandidate[] customers =
        [
            Candidate(id: 1, tier: new VipTier(), stays: [CampaignStart.AddDays(-10)]),
            Candidate(id: 2, stays: [CampaignStart.AddDays(-365)]),
            Candidate(id: 3, dateOfBirth: new DateTime(2000, 12, 24)),
        ];
        ISegmentStrategy[] strategies = [new VipSegment(), new LapsedGuestSegment(LapsedDays), new BirthdayMonthSegment()];

        // Act: the caller only knows ISegmentStrategy, as the campaign service will.
        int[][] chosen = strategies
            .Select(s => customers.Where(c => s.Includes(c, CampaignStart)).Select(c => c.CustomerId).ToArray())
            .ToArray();

        // Assert
        Assert.Equal([1], chosen[0]);
        Assert.Equal([2], chosen[1]);
        Assert.Equal([3], chosen[2]);
    }

    private static SegmentCandidate Candidate(
        int id = 1,
        MembershipTier? tier = null,
        DateTime? dateOfBirth = null,
        DateTime[]? stays = null)
    {
        return new SegmentCandidate(id, tier ?? new StandardTier(), dateOfBirth, stays ?? []);
    }
}
