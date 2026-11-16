namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// What the ticket subsystem is allowed to know about a customer: enough to pick one in a form
/// and to show who a ticket belongs to.
/// </summary>
/// <param name="Id">Customer Id, stored in the ticket as <c>CustomerId</c>.</param>
/// <param name="FullName">Customer name.</param>
/// <param name="Phone">Customer phone number.</param>
/// <param name="TierName">Name of the membership tier: Standard, Silver, Gold or VIP.</param>
public sealed record CustomerSummary(int Id, string FullName, string Phone, string TierName);
