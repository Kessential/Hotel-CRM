using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>
/// Chooses the candidate with the fewest active tickets among the agents of the handling department.
/// When no candidate belongs to that department, chooses among all candidates instead.
/// Ties go to the candidate that comes first in the list. Configuration name: <c>DepartmentMatch</c>.
/// </summary>
public sealed class DepartmentMatchStrategy : IAssignmentStrategy
{
    /// <inheritdoc />
    public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
    {
        throw new NotImplementedException();
    }
}
