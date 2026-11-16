namespace HotelCRM.Domain.Tickets;

/// <summary>
/// Urgency of a ticket, from lowest to highest. The order matters: rule 4 raises the priority by
/// moving up this list, never past <see cref="Urgent"/>. Each level has an SLA multiplier.
/// </summary>
public enum TicketPriority
{
    /// <summary>SLA multiplier 2.0.</summary>
    Low,

    /// <summary>SLA multiplier 1.0.</summary>
    Medium,

    /// <summary>SLA multiplier 0.5.</summary>
    High,

    /// <summary>SLA multiplier 0.25.</summary>
    Urgent,
}
