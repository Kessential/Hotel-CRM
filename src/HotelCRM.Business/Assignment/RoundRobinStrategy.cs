using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>
/// Hands tickets out in turn: chooses the first candidate whose Id is greater than the Id of the agent
/// chosen last time, and starts again from the lowest Id when there is none. The strategy remembers
/// the last chosen Id in a private field for as long as the application runs; it is not saved.
/// Configuration name: <c>RoundRobin</c>.
/// </summary>
public sealed class RoundRobinStrategy : IAssignmentStrategy
{
    /// <inheritdoc />
    public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
    {
        throw new NotImplementedException();
    }
}
