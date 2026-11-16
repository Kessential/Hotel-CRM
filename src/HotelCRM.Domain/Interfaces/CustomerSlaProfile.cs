namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// The two numbers a ticket needs from the membership tier of its customer (rule 4). Passing numbers
/// instead of the tier keeps the ticket subsystem independent of the customer subsystem.
/// </summary>
/// <param name="SlaFactor">Multiplier of the handling time: Standard 1.0, Silver 0.9, Gold 0.75, VIP 0.5.</param>
/// <param name="PriorityBoost">Priority levels added at creation: Standard 0, Silver 0, Gold 1, VIP 2.</param>
public sealed record CustomerSlaProfile(double SlaFactor, int PriorityBoost);
