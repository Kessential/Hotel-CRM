namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// Boundary interface: what the ticket subsystem (share A) needs to know about stays (share B).
/// An active stay is a booking whose status is CheckedIn.
/// Implemented by <c>StayLookup</c> in <c>Business/Queries</c>; tests use <c>FakeStayLookup</c>.
/// </summary>
public interface IStayLookup
{
    /// <summary>Rule 5: whether the customer currently has a CheckedIn booking for this room.</summary>
    bool IsRoomInActiveStay(int customerId, int roomId);

    /// <summary>
    /// Returns the rooms the customer is currently staying in (CheckedIn bookings):
    /// room Id mapped to room number. Empty when the customer is not staying.
    /// </summary>
    IReadOnlyDictionary<int, string> GetActiveRooms(int customerId);

    /// <summary>Returns the room number of a room, or an empty string when the room does not exist.</summary>
    string GetRoomNumber(int roomId);
}
