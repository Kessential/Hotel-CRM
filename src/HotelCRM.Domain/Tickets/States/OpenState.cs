using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Tickets.States;

/// <summary>A new ticket waiting to be assigned and started. Only Start is allowed.</summary>
public sealed class OpenState : TicketState
{
    /// <summary>Value of <see cref="Name"/> for this state.</summary>
    public const string StateName = "Open";

    /// <inheritdoc />
    public override string Name => StateName;

    /// <summary>True.</summary>
    public override bool IsActive => throw new NotImplementedException();

    /// <summary>False.</summary>
    public override bool IsClosed => throw new NotImplementedException();

    /// <summary>Rule 1: moves the ticket to <see cref="InProgressState"/>.</summary>
    /// <exception cref="BusinessRuleException">No agent is assigned to the ticket.</exception>
    internal override void Start(SupportTicket ticket, DateTime now)
    {
        throw new NotImplementedException();
    }
}
