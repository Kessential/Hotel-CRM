using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Domain.Tickets;

/// <summary>
/// The stored values of a ticket, with references already resolved. The data layer fills one of these
/// from a JSON record and passes it to the <c>Restore</c> method of the matching ticket class, which
/// rebuilds the ticket exactly as it was saved without running the creation rules again.
/// </summary>
internal sealed record TicketSnapshot(
    int Id,
    int CustomerId,
    string Title,
    string Description,
    TicketPriority Priority,
    DateTime CreatedAt,
    DateTime DueAt,
    DateTime UpdatedAt,
    TicketState State,
    Agent? AssignedAgent,
    DateTime? ResolvedAt,
    string? ResolutionNote,
    DateTime? ClosedAt);
