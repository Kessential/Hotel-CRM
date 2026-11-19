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
    private int _lastPickedId;

    /// <inheritdoc />
    public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
    {
        if (candidates.Count == 0)
        {
            return null;
        }

        Agent? next = candidates
            .Select(c => c.Agent)
            .Where(a => a.Id > _lastPickedId)
            .MinBy(a => a.Id);
        Agent picked = next ?? candidates.Select(c => c.Agent).MinBy(a => a.Id)!;

        _lastPickedId = picked.Id;
        return picked;
    }
}
