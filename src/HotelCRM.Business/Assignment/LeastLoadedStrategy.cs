using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>
/// Chooses the candidate with the fewest active tickets, whatever their department.
/// Ties go to the candidate that comes first in the list. Configuration name: <c>LeastLoaded</c>.
/// </summary>
public sealed class LeastLoadedStrategy : IAssignmentStrategy
{
    /// <inheritdoc />
    public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
    {
        throw new NotImplementedException();
    }
}
