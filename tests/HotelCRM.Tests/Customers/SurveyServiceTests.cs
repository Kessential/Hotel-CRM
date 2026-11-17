using HotelCRM.Business.Services;
using HotelCRM.Data;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Tests.Fakes;

namespace HotelCRM.Tests.Customers;

/// <summary>
/// Rule 9: a survey needs a resolved or closed ticket and can be recorded only once per ticket,
/// checked through <see cref="SurveyService"/> with an in-memory repository and a fake ticket query.
/// </summary>
public class SurveyServiceTests
{
    private const int TicketId = 1;

    private readonly InMemoryRepository<SatisfactionSurvey> _surveys = new();
    private readonly FakeTicketQuery _tickets = new();
    private readonly SurveyService _service;

    public SurveyServiceTests()
    {
        _service = new SurveyService(_surveys, _tickets);
    }

    [Fact]
    public void Submit_TicketNotResolved_ThrowsBusinessRule()
    {
        // Arrange: the fake does not report the ticket as resolved or closed.

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => _service.Submit(TicketId, 5, "Very helpful.", CustomerTestData.Now));
    }

    [Fact]
    public void Submit_SecondSurveyForTicket_ThrowsBusinessRule()
    {
        // Arrange: the ticket is resolved and has already been surveyed once.
        _tickets.MarkResolvedOrClosed(TicketId);
        _service.Submit(TicketId, 4, "Quick response.", CustomerTestData.Now);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => _service.Submit(TicketId, 5, "Changed my mind.", CustomerTestData.Now.AddHours(1)));
    }
}
