using HotelCRM.Domain.Common;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Tests.Tickets;

/// <summary>Rules 1 and 2: the state transitions of a ticket (State pattern).</summary>
public class TicketStateTests
{
    private static readonly DateTime _now = TicketTestData.Now;

    [Fact]
    public void Start_OpenTicketWithAgent_MovesToInProgress()
    {
        // Arrange
        SupportTicket ticket = TicketTestData.NewTicket();
        ticket.AssignTo(TicketTestData.NewAgent(2, TicketTestData.NewDepartment()), activeCount: 0, TicketTestData.MaxActive);

        // Act
        ticket.Start(_now);

        // Assert
        TicketState state = ticket.State;
        Assert.IsType<InProgressState>(state);
    }

    [Fact]
    public void Resolve_InProgressWithNote_MovesToResolved()
    {
        // Arrange
        SupportTicket ticket = TicketTestData.NewTicket();
        ticket.AssignTo(TicketTestData.NewAgent(2, TicketTestData.NewDepartment()), activeCount: 0, TicketTestData.MaxActive);
        ticket.Start(_now);
        DateTime resolvedAt = _now.AddHours(2);

        // Act
        ticket.Resolve("Towels delivered.", resolvedAt);

        // Assert
        TicketState state = ticket.State;
        Assert.IsType<ResolvedState>(state);
        Assert.Equal("Towels delivered.", ticket.ResolutionNote);
        Assert.Equal(resolvedAt, ticket.ResolvedAt);
    }

    [Fact]
    public void Close_ResolvedTicket_MovesToClosed()
    {
        // Arrange
        SupportTicket ticket = TicketTestData.NewResolvedTicket();
        DateTime closedAt = _now.AddDays(1);

        // Act
        ticket.Close(closedAt);

        // Assert
        TicketState state = ticket.State;
        Assert.IsType<ClosedState>(state);
        Assert.Equal(closedAt, ticket.ClosedAt);
    }

    [Fact]
    public void Start_WithoutAgent_ThrowsBusinessRule()
    {
        // Arrange
        SupportTicket ticket = TicketTestData.NewTicket();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => ticket.Start(_now));
    }

    [Fact]
    public void Resolve_ClosedTicket_ThrowsBusinessRule()
    {
        // Arrange
        SupportTicket ticket = TicketTestData.NewResolvedTicket();
        ticket.Close(_now.AddDays(1));

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => ticket.Resolve("Trying again.", _now.AddDays(2)));
    }

    [Fact]
    public void Reopen_AfterReopenDays_ThrowsBusinessRule()
    {
        // Arrange: resolved at "now"; the reopen window of 7 days ended a day ago.
        SupportTicket ticket = TicketTestData.NewResolvedTicket();
        DateTime tooLate = _now.AddDays(TicketTestData.ReopenDays + 1);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => ticket.Reopen(tooLate, TicketTestData.ReopenDays));
    }
}
