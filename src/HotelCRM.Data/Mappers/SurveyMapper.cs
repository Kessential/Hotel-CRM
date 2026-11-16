using HotelCRM.Data.Records;
using HotelCRM.Domain.Customers;

namespace HotelCRM.Data.Mappers;

/// <summary>Converts between <see cref="SatisfactionSurvey"/> and <see cref="SurveyRecord"/>.</summary>
public sealed class SurveyMapper : IMapper<SatisfactionSurvey, SurveyRecord>
{
    /// <inheritdoc />
    public SurveyRecord ToRecord(SatisfactionSurvey entity)
    {
        return new SurveyRecord
        {
            Id = entity.Id,
            TicketId = entity.TicketId,
            Score = entity.Score,
            Comment = entity.Comment,
            SubmittedAt = entity.SubmittedAt,
        };
    }

    /// <inheritdoc />
    public SatisfactionSurvey ToEntity(SurveyRecord record)
    {
        return SatisfactionSurvey.Restore(record.Id, record.TicketId, record.Score, record.Comment, record.SubmittedAt);
    }
}
