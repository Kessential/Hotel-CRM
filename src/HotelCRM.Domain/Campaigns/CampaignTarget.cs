using HotelCRM.Domain.Customers;

namespace HotelCRM.Domain.Campaigns;

/// <summary>
/// One customer on the list of a campaign (the n-n link between campaigns and customers).
/// Part of its <see cref="Campaign"/>: created and changed only through the campaign.
/// </summary>
public sealed class CampaignTarget
{
    internal CampaignTarget(int customerId, ContactChannel channel, DateTime sentAt)
    {
        CustomerId = customerId;
        Channel = channel;
        SentAt = sentAt;
    }

    /// <summary>Customer who received the offer.</summary>
    public int CustomerId { get; }

    /// <summary>Channel the offer was sent through.</summary>
    public ContactChannel Channel { get; }

    /// <summary>When the offer was sent.</summary>
    public DateTime SentAt { get; }

    /// <summary>Answer of the customer; null while there is none.</summary>
    public CampaignResponse? Response { get; private set; }

    /// <summary>When the answer was recorded; null while there is none.</summary>
    public DateTime? RespondedAt { get; private set; }

    /// <summary>Employee who recorded the answer; null while there is none.</summary>
    public int? RecordedById { get; private set; }

    /// <summary>Stores the answer. The campaign checks the rules before calling this.</summary>
    internal void Record(CampaignResponse response, int employeeId, DateTime now)
    {
        Response = response;
        RespondedAt = now;
        RecordedById = employeeId;
    }
}
