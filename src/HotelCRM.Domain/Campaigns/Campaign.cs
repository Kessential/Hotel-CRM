using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Campaigns;

/// <summary>
/// A care campaign sent to one customer segment over a date range. Owns its target list (composition)
/// and decides Q11: who may be added, once each, and when answers can still be recorded.
/// </summary>
public sealed class Campaign : IEntity
{
    private readonly List<CampaignTarget> _targets = new();

    /// <summary>Creates a campaign and checks its data. Dates keep only the day part.</summary>
    /// <exception cref="ValidationException">Name, segment or offer is empty, or the end date is before the start date.</exception>
    public Campaign(
        int id,
        string name,
        string segmentCode,
        string offer,
        DateTime startDate,
        DateTime endDate,
        int createdById,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tên chiến dịch không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(segmentCode))
        {
            throw new ValidationException("Phải chọn phân khúc khách hàng.");
        }

        if (string.IsNullOrWhiteSpace(offer))
        {
            throw new ValidationException("Nội dung ưu đãi không được để trống.");
        }

        if (endDate.Date < startDate.Date)
        {
            throw new ValidationException("Ngày kết thúc phải bằng hoặc sau ngày bắt đầu.");
        }

        Id = id;
        Name = name.Trim();
        SegmentCode = segmentCode.Trim();
        Offer = offer.Trim();
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        CreatedById = createdById;
        CreatedAt = createdAt;
    }

    /// <inheritdoc />
    public int Id { get; }

    /// <summary>Name of the campaign.</summary>
    public string Name { get; }

    /// <summary>Code of the segment strategy that chose the customers.</summary>
    public string SegmentCode { get; }

    /// <summary>Offer sent to the customers.</summary>
    public string Offer { get; }

    /// <summary>First day of the campaign.</summary>
    public DateTime StartDate { get; }

    /// <summary>Last day of the campaign; answers can be recorded until the end of this day.</summary>
    public DateTime EndDate { get; }

    /// <summary>Employee who created the campaign.</summary>
    public int CreatedById { get; }

    /// <summary>When the campaign was created.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>Customers the offer was sent to.</summary>
    public IReadOnlyList<CampaignTarget> Targets => _targets.AsReadOnly();

    /// <summary>Share of targets that answered, from 0 to 1; 0 when the list is empty.</summary>
    public decimal ResponseRate =>
        _targets.Count == 0 ? 0m : (decimal)_targets.Count(t => t.Response is not null) / _targets.Count;

    /// <summary>Whether the campaign is over at <paramref name="now"/> (the day after <see cref="EndDate"/> or later).</summary>
    public bool IsEnded(DateTime now)
    {
        return now.Date > EndDate;
    }

    /// <summary>
    /// Q11: whether a customer of the segment may receive the offer: the customer agreed to marketing
    /// and has no complaint that is still open. The service uses it to filter the segment.
    /// </summary>
    public static bool IsEligible(bool marketingConsent, bool hasOpenComplaint)
    {
        return marketingConsent && !hasOpenComplaint;
    }

    /// <summary>Q11: adds a customer to the list and records that the offer was sent at <paramref name="now"/>.</summary>
    /// <exception cref="BusinessRuleException">
    /// The campaign is over, the customer is not eligible (see <see cref="IsEligible"/>) or is already on the list.
    /// </exception>
    public CampaignTarget AddTarget(
        int customerId,
        bool marketingConsent,
        bool hasOpenComplaint,
        ContactChannel channel,
        DateTime now)
    {
        EnsureNotEnded(now);

        if (!marketingConsent)
        {
            throw new BusinessRuleException($"Khách {customerId} không đồng ý nhận ưu đãi.");
        }

        if (hasOpenComplaint)
        {
            throw new BusinessRuleException($"Khách {customerId} đang có khiếu nại chưa đóng.");
        }

        if (FindTarget(customerId) is not null)
        {
            throw new BusinessRuleException($"Khách {customerId} đã có trong danh sách của chiến dịch này.");
        }

        CampaignTarget target = new(customerId, channel, now);
        _targets.Add(target);
        return target;
    }

    /// <summary>Q11: a campaign needs at least one customer. The service calls this before saving.</summary>
    /// <exception cref="BusinessRuleException">The target list is empty.</exception>
    public void EnsureHasTargets()
    {
        if (_targets.Count == 0)
        {
            throw new BusinessRuleException(
                $"Chiến dịch \"{Name}\" chưa có khách nào phù hợp; hãy đổi phân khúc hoặc khoảng ngày.");
        }
    }

    /// <summary>
    /// Q11: records or replaces the answer of a targeted customer while the campaign is not over
    /// (for example Interested first, Booked later).
    /// </summary>
    /// <exception cref="BusinessRuleException">The campaign is over or the customer is not on the list.</exception>
    public void RecordResponse(int customerId, CampaignResponse response, int employeeId, DateTime now)
    {
        EnsureNotEnded(now);

        CampaignTarget target = FindTarget(customerId)
            ?? throw new BusinessRuleException($"Khách {customerId} không có trong danh sách của chiến dịch này.");
        target.Record(response, employeeId, now);
    }

    private CampaignTarget? FindTarget(int customerId)
    {
        return _targets.FirstOrDefault(t => t.CustomerId == customerId);
    }

    private void EnsureNotEnded(DateTime now)
    {
        if (IsEnded(now))
        {
            throw new BusinessRuleException($"Chiến dịch \"{Name}\" đã kết thúc ngày {EndDate:dd/MM/yyyy}.");
        }
    }
}
