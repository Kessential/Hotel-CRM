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
    public override bool IsActive => true;

    /// <summary>False.</summary>
    public override bool IsClosed => false;

    /// <summary>Rule 1: moves the ticket to <see cref="InProgressState"/>.</summary>
    /// <exception cref="BusinessRuleException">No agent is assigned to the ticket.</exception>
    internal override void Start(SupportTicket ticket, DateTime now)
    {
        if (ticket.AssignedAgent is null)
        {
            throw new BusinessRuleException("The ticket must be assigned to an agent before it can be started.");
        }

        ticket.MoveTo(new InProgressState(), now);
    }
}
