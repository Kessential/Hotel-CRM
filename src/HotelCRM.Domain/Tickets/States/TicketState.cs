using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Tickets.States;

/// <summary>
/// State of a ticket (State pattern). <see cref="SupportTicket"/> forwards Start, Resolve, Close and
/// Reopen to its current state. The base class rejects every action; each concrete state overrides
/// only the actions that are legal in that state (rule 1) and moves the ticket to the next state
/// with <see cref="SupportTicket.MoveTo"/>.
/// </summary>
public abstract class TicketState
{
    /// <summary>Name of the state: <c>Open</c>, <c>InProgress</c>, <c>Resolved</c> or <c>Closed</c>. Stored in <c>tickets.json</c>.</summary>
    public abstract string Name { get; }

    /// <summary>
    /// Whether the ticket is still being worked on: true for Open and InProgress.
    /// Active tickets count towards the agent's limit (rule 3) and can be assigned or reassigned (rule 1).
    /// </summary>
    public abstract bool IsActive { get; }

    /// <summary>Whether the ticket is finished for good: true for Closed only. Rule 10 looks at tickets that are not closed.</summary>
    public abstract bool IsClosed { get; }

    /// <summary>
    /// Returns the state object for a stored name. Adding a new state means adding one line here.
    /// </summary>
    /// <exception cref="ArgumentException">The name is not a known state (corrupt data file).</exception>
    public static TicketState FromName(string name)
    {
        return name switch
        {
            OpenState.StateName => new OpenState(),
            InProgressState.StateName => new InProgressState(),
            ResolvedState.StateName => new ResolvedState(),
            ClosedState.StateName => new ClosedState(),
            _ => throw new ArgumentException($"Unknown ticket state '{name}'.", nameof(name)),
        };
    }

    /// <summary>Returns <see cref="Name"/>.</summary>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>Default: starting is not allowed in this state.</summary>
    /// <exception cref="BusinessRuleException">Always, unless overridden.</exception>
    internal virtual void Start(SupportTicket ticket, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>Default: resolving is not allowed in this state.</summary>
    /// <exception cref="BusinessRuleException">Always, unless overridden.</exception>
    internal virtual void Resolve(SupportTicket ticket, string note, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>Default: closing is not allowed in this state.</summary>
    /// <exception cref="BusinessRuleException">Always, unless overridden.</exception>
    internal virtual void Close(SupportTicket ticket, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>Default: reopening is not allowed in this state.</summary>
    /// <exception cref="BusinessRuleException">Always, unless overridden.</exception>
    internal virtual void Reopen(SupportTicket ticket, DateTime now, int reopenDays)
    {
        throw new NotImplementedException();
    }
}
