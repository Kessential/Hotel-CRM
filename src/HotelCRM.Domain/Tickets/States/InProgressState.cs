using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Tickets.States;

/// <summary>A ticket the assigned agent is working on. Only Resolve is allowed.</summary>
public sealed class InProgressState : TicketState
{
    /// <summary>Value of <see cref="Name"/> for this state.</summary>
    public const string StateName = "InProgress";

    /// <inheritdoc />
    public override string Name => StateName;

    /// <summary>True.</summary>
    public override bool IsActive => throw new NotImplementedException();

    /// <summary>False.</summary>
    public override bool IsClosed => throw new NotImplementedException();

    /// <summary>
    /// Rule 1: records the note and time with <see cref="SupportTicket.RecordResolution"/> and moves
    /// the ticket to <see cref="ResolvedState"/>.
    /// </summary>
    /// <exception cref="ValidationException">The note is empty.</exception>
    internal override void Resolve(SupportTicket ticket, string note, DateTime now)
    {
        throw new NotImplementedException();
    }
}
