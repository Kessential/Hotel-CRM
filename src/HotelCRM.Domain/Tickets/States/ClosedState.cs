namespace HotelCRM.Domain.Tickets.States;

/// <summary>
/// A finished ticket. Closed is final: this class overrides no action, so every action falls back
/// to the base class and is rejected (rule 1).
/// </summary>
public sealed class ClosedState : TicketState
{
    /// <summary>Value of <see cref="Name"/> for this state.</summary>
    public const string StateName = "Closed";

    /// <inheritdoc />
    public override string Name => StateName;

    /// <summary>False.</summary>
    public override bool IsActive => throw new NotImplementedException();

    /// <summary>True.</summary>
    public override bool IsClosed => throw new NotImplementedException();
}
