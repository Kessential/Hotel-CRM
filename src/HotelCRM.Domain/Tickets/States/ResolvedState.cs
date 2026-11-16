using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Tickets.States;

/// <summary>A ticket the agent has finished. It can be closed, or reopened within the reopen window.</summary>
public sealed class ResolvedState : TicketState
{
    /// <summary>Value of <see cref="Name"/> for this state.</summary>
    public const string StateName = "Resolved";

    /// <inheritdoc />
    public override string Name => StateName;

    /// <summary>False.</summary>
    public override bool IsActive => throw new NotImplementedException();

    /// <summary>False.</summary>
    public override bool IsClosed => throw new NotImplementedException();

    /// <summary>
    /// Rule 1: records the time with <see cref="SupportTicket.RecordClosure"/> and moves the ticket
    /// to <see cref="ClosedState"/>.
    /// </summary>
    internal override void Close(SupportTicket ticket, DateTime now)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Rule 2: moves the ticket back to <see cref="InProgressState"/> when
    /// <c>now &lt;= ticket.ResolvedAt + reopenDays</c>. The agent stays assigned.
    /// </summary>
    /// <exception cref="BusinessRuleException">More than <paramref name="reopenDays"/> days have passed since the resolution.</exception>
    internal override void Reopen(SupportTicket ticket, DateTime now, int reopenDays)
    {
        throw new NotImplementedException();
    }
}
