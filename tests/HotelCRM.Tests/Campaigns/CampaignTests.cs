using HotelCRM.Domain.Campaigns;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Tests.Campaigns;

/// <summary>Q11: who may join a campaign, once each, and answers only until the end date.</summary>
public class CampaignTests
{
    private const int ManagerId = 2;
    private const int AgentId = 4;
    private static readonly DateTime Start = new(2026, 12, 1);
    private static readonly DateTime End = new(2026, 12, 31);
    private static readonly DateTime DuringCampaign = new(2026, 12, 10, 9, 0, 0);

    [Fact]
    public void Constructor_EndBeforeStart_ThrowsValidation()
    {
        // Act + Assert
        Assert.Throws<ValidationException>(
            () => new Campaign(1, "Tri ân cuối năm", "VIP", "Giảm 20%", End, Start, ManagerId, Start));
    }

    [Fact]
    public void AddTarget_SameCustomerTwice_ThrowsBusinessRule()
    {
        // Arrange
        Campaign campaign = NewCampaign();
        campaign.AddTarget(7, marketingConsent: true, hasOpenComplaint: false, ContactChannel.Email, DuringCampaign);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => campaign.AddTarget(7, marketingConsent: true, hasOpenComplaint: false, ContactChannel.Phone, DuringCampaign));
        Assert.Single(campaign.Targets);
    }

    [Fact]
    public void AddTarget_NoMarketingConsent_ThrowsBusinessRule()
    {
        // Arrange
        Campaign campaign = NewCampaign();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => campaign.AddTarget(7, marketingConsent: false, hasOpenComplaint: false, ContactChannel.Email, DuringCampaign));
    }

    [Fact]
    public void AddTarget_OpenComplaint_ThrowsBusinessRule()
    {
        // Arrange
        Campaign campaign = NewCampaign();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => campaign.AddTarget(7, marketingConsent: true, hasOpenComplaint: true, ContactChannel.Email, DuringCampaign));
    }

    [Fact]
    public void RecordResponse_LastMinuteOfEndDate_RecordsAnswer()
    {
        // Arrange
        Campaign campaign = NewCampaign();
        campaign.AddTarget(7, marketingConsent: true, hasOpenComplaint: false, ContactChannel.Phone, DuringCampaign);
        DateTime lastMinute = End.AddDays(1).AddMinutes(-1);

        // Act
        campaign.RecordResponse(7, CampaignResponse.Booked, AgentId, lastMinute);

        // Assert
        Assert.Equal(CampaignResponse.Booked, campaign.Targets[0].Response);
        Assert.Equal(AgentId, campaign.Targets[0].RecordedById);
    }

    [Fact]
    public void RecordResponse_DayAfterEndDate_ThrowsBusinessRule()
    {
        // Arrange
        Campaign campaign = NewCampaign();
        campaign.AddTarget(7, marketingConsent: true, hasOpenComplaint: false, ContactChannel.Phone, DuringCampaign);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => campaign.RecordResponse(7, CampaignResponse.Booked, AgentId, End.AddDays(1)));
        Assert.Null(campaign.Targets[0].Response);
    }

    [Fact]
    public void RecordResponse_CustomerNotOnList_ThrowsBusinessRule()
    {
        // Arrange
        Campaign campaign = NewCampaign();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => campaign.RecordResponse(7, CampaignResponse.Interested, AgentId, DuringCampaign));
    }

    [Fact]
    public void ResponseRate_OneAnswerOutOfFour_IsOneQuarter()
    {
        // Arrange
        Campaign campaign = NewCampaign();
        foreach (int customerId in new[] { 7, 8, 9, 10 })
        {
            campaign.AddTarget(customerId, marketingConsent: true, hasOpenComplaint: false, ContactChannel.Email, DuringCampaign);
        }

        // Act
        campaign.RecordResponse(8, CampaignResponse.Declined, AgentId, DuringCampaign);

        // Assert
        Assert.Equal(0.25m, campaign.ResponseRate);
    }

    private static Campaign NewCampaign()
    {
        return new Campaign(1, "Tri ân cuối năm", "VIP", "Giảm 20% giá phòng", Start, End, ManagerId, Start.AddDays(-3));
    }
}
