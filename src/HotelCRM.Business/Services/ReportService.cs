using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Services;

/// <summary>
/// UC10: reports for the manager. Read-only. Reads tickets only through <see cref="ITicketQuery"/>.
/// Definitions: a ticket is overdue when <c>(ResolvedAt ?? now) &gt; DueAt</c>; resolution hours are
/// <c>ResolvedAt - CreatedAt</c> of tickets resolved at least once; a survey belongs to the agent and
/// department of the ticket it rates.
/// </summary>
public sealed class ReportService
{
    private readonly ITicketQuery _tickets;
    private readonly IRepository<Employee> _employees;
    private readonly IRepository<Department> _departments;
    private readonly IRepository<SatisfactionSurvey> _surveys;

    /// <summary>Creates the service.</summary>
    /// <param name="tickets">Ticket lookups of the other subsystem.</param>
    /// <param name="employees">Employee repository; agents are the employees of type <see cref="Agent"/>.</param>
    /// <param name="departments">Department repository.</param>
    /// <param name="surveys">Survey repository.</param>
    public ReportService(
        ITicketQuery tickets,
        IRepository<Employee> employees,
        IRepository<Department> departments,
        IRepository<SatisfactionSurvey> surveys)
    {
        _tickets = tickets;
        _employees = employees;
        _departments = departments;
        _surveys = surveys;
    }

    /// <summary>UC10: returns one row per agent, ordered by agent name; agents without tickets are included with zeros.</summary>
    /// <param name="now">Current time, supplied by the form; used to decide which unresolved tickets are overdue.</param>
    public IReadOnlyList<AgentPerformanceRow> AgentPerformance(DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>UC10: returns one row per department, ordered by department name; departments without tickets are included with zeros.</summary>
    public IReadOnlyList<DepartmentReportRow> ByDepartment()
    {
        throw new NotImplementedException();
    }
}
