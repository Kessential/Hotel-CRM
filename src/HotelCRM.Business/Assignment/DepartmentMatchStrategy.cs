using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>
/// Chooses the candidate with the fewest active tickets among the agents of the handling department.
/// When no candidate belongs to that department, chooses among all candidates instead.
/// Ties go to the candidate that comes first in the list. Configuration name: <c>DepartmentMatch</c>.
/// </summary>
public sealed class DepartmentMatchStrategy : IAssignmentStrategy
{
    private readonly LeastLoadedStrategy _leastLoaded = new();

    /// <inheritdoc />
    public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
    {
        List<AgentWorkload> inDepartment = candidates
            .Where(c => string.Equals(c.Agent.Department.Code, departmentCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return _leastLoaded.Pick(departmentCode, inDepartment.Count > 0 ? inDepartment : candidates);
    }
}
