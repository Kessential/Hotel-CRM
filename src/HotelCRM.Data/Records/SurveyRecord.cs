namespace HotelCRM.Data.Records;

/// <summary>One entry of <c>surveys.json</c>. The ticket is stored by Id.</summary>
public sealed class SurveyRecord
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int Score { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }
}
