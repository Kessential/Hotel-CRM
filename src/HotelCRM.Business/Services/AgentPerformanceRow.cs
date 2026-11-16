namespace HotelCRM.Business.Services;

/// <summary>One line of the agent performance report (UC10): the figures of one agent.</summary>
/// <param name="AgentName">Full name of the agent.</param>
/// <param name="DepartmentName">Name of the agent's department.</param>
/// <param name="AssignedCount">Tickets currently assigned to the agent, in any state.</param>
/// <param name="ResolvedCount">Of those, tickets that have been resolved at least once (<c>ResolvedAt</c> is set).</param>
/// <param name="AverageResolutionHours">Average of <c>SupportTicket.ResolutionHours</c> over the resolved tickets; null when there are none.</param>
/// <param name="OverdueCount">Of the assigned tickets, those for which <c>SupportTicket.IsOverdue(now)</c> is true.</param>
/// <param name="AverageSurveyScore">Average score of the surveys on the agent's tickets; null when there are none.</param>
public sealed record AgentPerformanceRow(
    string AgentName,
    string DepartmentName,
    int AssignedCount,
    int ResolvedCount,
    double? AverageResolutionHours,
    int OverdueCount,
    double? AverageSurveyScore);
