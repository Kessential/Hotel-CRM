using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>
/// Algorithm that chooses the agent for a ticket (Strategy pattern). <c>TicketService.AutoAssign</c>
/// is the context: it knows only this interface. The strategy in use is chosen by
/// <c>DefaultAssignmentStrategy</c> in <c>appsettings.json</c> through a lookup table built in <c>Program.cs</c>.
/// </summary>
public interface IAssignmentStrategy
{
    /// <summary>Chooses an agent for a ticket.</summary>
    /// <param name="departmentCode">
    /// Code of the department that handles the ticket (<c>SupportTicket.HandlingDepartmentCode</c>).
    /// A strategy may ignore it.
    /// </param>
    /// <param name="candidates">
    /// Agents who can still take a ticket (below <c>MaxActiveTickets</c>), each with their current
    /// number of active tickets, in repository order. May be empty.
    /// </param>
    /// <returns>The chosen agent, or null when <paramref name="candidates"/> is empty.</returns>
    Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates);
}
