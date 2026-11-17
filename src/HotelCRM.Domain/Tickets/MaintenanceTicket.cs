using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Tickets;

/// <summary>
/// A report of something broken in a room. Handled by Engineering within a base time of 12 hours.
/// Rule 5: the room must be one the customer is currently staying in.
/// </summary>
public sealed class MaintenanceTicket : SupportTicket
{
    /// <summary>Value of <see cref="TypeName"/> for this class.</summary>
    public const string TypeKey = "Maintenance";

    /// <summary>Creates a new maintenance ticket; see the base constructor for the creation rules.</summary>
    /// <param name="roomId">Id of the room that needs repair; required.</param>
    /// <exception cref="ValidationException">The title is empty, or <paramref name="roomId"/> is null.</exception>
    public MaintenanceTicket(
        int id,
        int customerId,
        string title,
        string description,
        TicketPriority requestedPriority,
        CustomerSlaProfile slaProfile,
        DateTime createdAt,
        int? roomId)
        : base(id, customerId, title, description, requestedPriority, slaProfile, createdAt)
    {
        RoomId = roomId ?? 0;
        EnsureRoomGiven(roomId);
    }

    private MaintenanceTicket(TicketSnapshot snapshot, int roomId)
        : base(snapshot)
    {
        RoomId = roomId;
    }

    /// <summary>Id of the room that needs repair. Only the Id is kept: the room belongs to the other subsystem.</summary>
    public int RoomId { get; }

    /// <inheritdoc />
    public override string TypeName => TypeKey;

    /// <summary>12 hours.</summary>
    public override int BaseSlaHours => 12;

    /// <summary><c>ENG</c> (Engineering).</summary>
    public override string HandlingDepartmentCode => "ENG";

    /// <summary>Rule 5: the room must belong to a CheckedIn booking of the ticket's customer.</summary>
    /// <param name="roomInActiveStay">Result of <see cref="IStayLookup.IsRoomInActiveStay"/> for <c>CustomerId</c> and <see cref="RoomId"/>.</param>
    /// <exception cref="BusinessRuleException"><paramref name="roomInActiveStay"/> is false.</exception>
    public override void Validate(bool roomInActiveStay)
    {
        if (!roomInActiveStay)
        {
            throw new BusinessRuleException(
                "The room of a maintenance ticket must belong to the customer's current stay (checked in).");
        }
    }

    /// <summary>Rebuilds a maintenance ticket from stored data.</summary>
    internal static MaintenanceTicket Restore(TicketSnapshot snapshot, int roomId)
    {
        return new MaintenanceTicket(snapshot, roomId);
    }

    /// <summary>Throws <see cref="ValidationException"/> when no room was given.</summary>
    private static void EnsureRoomGiven(int? roomId)
    {
        if (roomId is null)
        {
            throw new ValidationException("A maintenance ticket must name the room that needs repair.");
        }
    }
}
