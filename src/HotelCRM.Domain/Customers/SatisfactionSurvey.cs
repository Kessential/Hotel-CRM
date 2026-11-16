using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// The customer's rating of how a ticket was handled. Refers to the ticket by Id only.
/// Created through <see cref="Create"/>, which enforces rule 9.
/// </summary>
public sealed class SatisfactionSurvey : IEntity
{
    /// <summary>Lowest score.</summary>
    public const int MinScore = 1;

    /// <summary>Highest score.</summary>
    public const int MaxScore = 5;

    private SatisfactionSurvey(int id, int ticketId, int score, string comment, DateTime submittedAt)
    {
        Id = id;
        TicketId = ticketId;
        Score = score;
        Comment = comment;
        SubmittedAt = submittedAt;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Id of the surveyed ticket. At most one survey exists per ticket.</summary>
    public int TicketId { get; }

    /// <summary>Score from <see cref="MinScore"/> to <see cref="MaxScore"/>.</summary>
    public int Score { get; }

    /// <summary>Optional comment of the customer.</summary>
    public string Comment { get; }

    /// <summary>When the survey was recorded.</summary>
    public DateTime SubmittedAt { get; }

    /// <summary>Rule 9: creates a survey for a ticket.</summary>
    /// <param name="id">Id obtained from the survey repository.</param>
    /// <param name="ticketId">Id of the surveyed ticket.</param>
    /// <param name="score">Score from 1 to 5.</param>
    /// <param name="comment">Optional comment; may be empty.</param>
    /// <param name="submittedAt">Time of recording (the caller's "now").</param>
    /// <param name="ticketResolvedOrClosed">Result of <c>ITicketQuery.IsResolvedOrClosed</c> for the ticket.</param>
    /// <param name="alreadySurveyed">Whether a survey for this ticket already exists; looked up by the service.</param>
    /// <exception cref="ValidationException">The score is outside 1 to 5.</exception>
    /// <exception cref="BusinessRuleException">
    /// The ticket is not Resolved or Closed, or it has already been surveyed.
    /// </exception>
    public static SatisfactionSurvey Create(
        int id,
        int ticketId,
        int score,
        string comment,
        DateTime submittedAt,
        bool ticketResolvedOrClosed,
        bool alreadySurveyed)
    {
        SatisfactionSurvey survey = new(id, ticketId, score, comment, submittedAt);
        survey.EnsureAllowed(ticketResolvedOrClosed, alreadySurveyed);
        return survey;
    }

    /// <summary>Rebuilds a survey from stored data.</summary>
    internal static SatisfactionSurvey Restore(int id, int ticketId, int score, string comment, DateTime submittedAt)
    {
        return new SatisfactionSurvey(id, ticketId, score, comment, submittedAt);
    }

    /// <summary>Checks the score range and the two looked-up conditions of rule 9.</summary>
    private void EnsureAllowed(bool ticketResolvedOrClosed, bool alreadySurveyed)
    {
        throw new NotImplementedException();
    }
}
