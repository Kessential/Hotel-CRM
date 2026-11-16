namespace HotelCRM.Business.Services;

/// <summary>
/// One line of the department report (UC10): the figures of one department. A ticket belongs to the
/// department of the agent it is assigned to; unassigned tickets are not counted.
/// </summary>
/// <param name="DepartmentName">Name of the department.</param>
/// <param name="OpenCount">Tickets in state Open.</param>
/// <param name="InProgressCount">Tickets in state InProgress.</param>
/// <param name="ResolvedCount">Tickets in state Resolved.</param>
/// <param name="ClosedCount">Tickets in state Closed.</param>
/// <param name="AverageResolutionHours">Average of <c>SupportTicket.ResolutionHours</c> over the tickets resolved at least once; null when there are none.</param>
/// <param name="AverageSurveyScore">Average score of the surveys on the department's tickets; null when there are none.</param>
public sealed record DepartmentReportRow(
    string DepartmentName,
    int OpenCount,
    int InProgressCount,
    int ResolvedCount,
    int ClosedCount,
    double? AverageResolutionHours,
    double? AverageSurveyScore);
