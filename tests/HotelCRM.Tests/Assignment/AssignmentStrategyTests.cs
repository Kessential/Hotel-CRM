using HotelCRM.Business.Assignment;
using HotelCRM.Domain.Staff;

namespace HotelCRM.Tests.Assignment;

/// <summary>Strategy pattern: each assignment algorithm is used through <see cref="IAssignmentStrategy"/>.</summary>
public class AssignmentStrategyTests
{
    [Fact]
    public void LeastLoaded_Pick_ReturnsAgentWithFewestActive()
    {
        // Arrange: three agents holding 3, 1 and 2 active tickets.
        Department department = new(1, "GUEST", "Guest Relations");
        Agent busy = NewAgent(1, department);
        Agent free = NewAgent(2, department);
        Agent average = NewAgent(3, department);
        IReadOnlyList<AgentWorkload> candidates = new[]
        {
            new AgentWorkload(busy, 3),
            new AgentWorkload(free, 1),
            new AgentWorkload(average, 2),
        };
        IAssignmentStrategy strategy = new LeastLoadedStrategy();

        // Act
        Agent? picked = strategy.Pick(department.Code, candidates);

        // Assert
        Assert.Same(free, picked);
    }

    private static Agent NewAgent(int id, Department department)
    {
        return new Agent(id, $"Agent {id}", "0901000002", $"agent{id}@hotel.example", $"agent{id}", "hash", "salt", department);
    }
}
