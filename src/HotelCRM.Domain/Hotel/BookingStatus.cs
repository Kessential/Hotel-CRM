namespace HotelCRM.Domain.Hotel;

/// <summary>
/// Life cycle of a booking: Reserved, then CheckedIn, then CheckedOut; a Reserved booking can instead be Cancelled.
/// </summary>
public enum BookingStatus
{
    /// <summary>Room is reserved; the guest has not arrived. Blocks the room for its dates.</summary>
    Reserved,

    /// <summary>The guest is staying (an "active stay"). Blocks the room for its dates.</summary>
    CheckedIn,

    /// <summary>The guest has left; loyalty points were awarded.</summary>
    CheckedOut,

    /// <summary>The reservation was cancelled before check-in.</summary>
    Cancelled,
}
