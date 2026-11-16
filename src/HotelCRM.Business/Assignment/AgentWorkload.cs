using HotelCRM.Domain.Staff;

namespace HotelCRM.Business.Assignment;

/// <summary>An agent together with the number of tickets they are currently working on.</summary>
/// <param name="Agent">The agent.</param>
/// <param name="ActiveCount">Tickets assigned to the agent that are Open or InProgress.</param>
public sealed record AgentWorkload(Agent Agent, int ActiveCount);
