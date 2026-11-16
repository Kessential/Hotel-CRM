using HotelCRM.Domain.Tickets;

namespace HotelCRM.Data.Records;

/// <summary>
/// One entry of <c>tickets.json</c>. <see cref="Type"/> (<c>Complaint</c>, <c>ServiceRequest</c>,
/// <c>Maintenance</c>) tells the mapper which subclass to rebuild; <see cref="State"/> holds the state
/// name (<c>Open</c>, <c>InProgress</c>, <c>Resolved</c>, <c>Closed</c>). The agent is stored by Id.
/// <see cref="RoomId"/> is set for maintenance tickets only.
/// </summary>
public sealed class TicketRecord
{
    public int Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime DueAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string State { get; set; } = string.Empty;

    public int? AssignedAgentId { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string? ResolutionNote { get; set; }

    public DateTime? ClosedAt { get; set; }

    public int? RoomId { get; set; }
}
