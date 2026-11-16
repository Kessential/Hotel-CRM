using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Business.Services;

/// <summary>
/// UC8: record the customer's satisfaction with a ticket. Reaches the ticket subsystem only through
/// <see cref="ITicketQuery"/>.
/// </summary>
public sealed class SurveyService
{
    private readonly IRepository<SatisfactionSurvey> _surveys;
    private readonly ITicketQuery _tickets;

    /// <summary>Creates the service.</summary>
    /// <param name="surveys">Survey repository.</param>
    /// <param name="tickets">Ticket lookups of the other subsystem.</param>
    public SurveyService(IRepository<SatisfactionSurvey> surveys, ITicketQuery tickets)
    {
        _surveys = surveys;
        _tickets = tickets;
    }

    /// <summary>
    /// UC8: records a survey. Steps: ask <see cref="ITicketQuery.IsResolvedOrClosed"/>; look up whether a
    /// survey for the ticket already exists; take the next Id; <see cref="SatisfactionSurvey.Create"/>
    /// decides (rule 9); save.
    /// </summary>
    /// <param name="ticketId">Id of the surveyed ticket.</param>
    /// <param name="score">Score from 1 to 5.</param>
    /// <param name="comment">Optional comment; may be empty.</param>
    /// <param name="now">Current time, supplied by the form.</param>
    /// <returns>The saved survey.</returns>
    /// <exception cref="ValidationException">The score is outside 1 to 5.</exception>
    /// <exception cref="BusinessRuleException">The ticket is not Resolved or Closed, or has already been surveyed.</exception>
    public SatisfactionSurvey Submit(int ticketId, int score, string comment, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the customer's tickets that can be surveyed now: Resolved or Closed and without a survey.
    /// Uses <see cref="ITicketQuery.GetByCustomer"/> and <see cref="ITicketQuery.IsResolvedOrClosed"/>.
    /// </summary>
    public IReadOnlyList<SupportTicket> GetSurveyableTickets(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <summary>Returns the survey of a ticket, or null when the ticket has not been surveyed.</summary>
    public SatisfactionSurvey? FindByTicket(int ticketId)
    {
        throw new NotImplementedException();
    }
}
