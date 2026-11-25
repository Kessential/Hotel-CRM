namespace HotelCRM.Domain.Campaigns;

/// <summary>What a targeted customer answered to a campaign offer.</summary>
public enum CampaignResponse
{
    /// <summary>The customer is interested but has not booked yet.</summary>
    Interested,

    /// <summary>The customer booked a stay because of the offer.</summary>
    Booked,

    /// <summary>The customer declined the offer.</summary>
    Declined,
}
