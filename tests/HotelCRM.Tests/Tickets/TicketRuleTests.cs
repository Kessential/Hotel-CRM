using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Tests.Tickets;

/// <summary>Rule 3 (agent workload limit) and rule 4 (priority boost and due time).</summary>
public class TicketRuleTests
{
    private static readonly DateTime _now = TicketTestData.Now;

    [Fact]
    public void AssignTo_AgentAtMaxActive_ThrowsBusinessRule()
    {
        // Arrange: the agent already holds as many active tickets as allowed.
        SupportTicket ticket = TicketTestData.NewTicket();
        Agent agent = TicketTestData.NewAgent(2, TicketTestData.NewDepartment());

        // Act + Assert
        Assert.Throws<BusinessRuleException>(
            () => ticket.AssignTo(agent, activeCount: TicketTestData.MaxActive, TicketTestData.MaxActive));
    }

    [Fact]
    public void CalculateDueAt_ComplaintBoostedToHighFactor075_Returns9Hours()
    {
        // Arrange: a Gold customer (factor 0.75, boost 1) files a Medium complaint.
        CustomerSlaProfile gold = new(SlaFactor: 0.75, PriorityBoost: 1);
        SupportTicket ticket = new ComplaintTicket(
            1, TicketTestData.CustomerId, "Noisy room", "Loud music next door all night.", TicketPriority.Medium, gold, _now);

        // Act
        DateTime dueAt = ticket.CalculateDueAt(gold.SlaFactor);

        // Assert: Medium is raised to High; 24 h x 0.5 x 0.75 = 9 h.
        Assert.Equal(TicketPriority.High, ticket.Priority);
        Assert.Equal(_now.AddHours(9), dueAt);
        Assert.Equal(dueAt, ticket.DueAt);
    }
}
