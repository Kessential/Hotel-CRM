using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>A physical room of the hotel. Decides whether it is free for a date range (Q8).</summary>
public sealed class Room : IEntity
{
    /// <summary>Creates a room and registers it in its room type.</summary>
    /// <param name="id">Id assigned by the room repository.</param>
    /// <param name="number">Room number shown to staff and guests, for example "305"; required.</param>
    /// <param name="floor">Floor the room is on; must be greater than 0.</param>
    /// <param name="roomType">Category and current price of the room.</param>
    /// <exception cref="ValidationException">The number is empty or the floor is not positive.</exception>
    public Room(int id, string number, int floor, RoomType roomType)
    {
        ArgumentNullException.ThrowIfNull(roomType);
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ValidationException("Số phòng không được để trống.");
        }

        if (floor <= 0)
        {
            throw new ValidationException("Tầng phải lớn hơn 0.");
        }

        Id = id;
        Number = number.Trim();
        Floor = floor;
        RoomType = roomType;
        roomType.AddRoom(this);
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Room number; never empty.</summary>
    public string Number { get; }

    /// <summary>Floor the room is on.</summary>
    public int Floor { get; }

    /// <summary>Category and current price of the room.</summary>
    public RoomType RoomType { get; }

    /// <summary>
    /// Q8: whether the room is free for the nights from <paramref name="checkInDate"/> up to, but not including,
    /// <paramref name="checkOutDate"/>. Only bookings of this room that still block it (Reserved or CheckedIn)
    /// count, so a cancelled booking frees its dates. Ranges are half-open, so a guest may check in on the
    /// day another one checks out.
    /// </summary>
    /// <param name="checkInDate">First night of the requested stay.</param>
    /// <param name="checkOutDate">Departure day of the requested stay.</param>
    /// <param name="bookings">Existing bookings; bookings of other rooms are ignored.</param>
    public bool IsAvailable(DateTime checkInDate, DateTime checkOutDate, IEnumerable<Booking> bookings)
    {
        return FindConflict(checkInDate, checkOutDate, bookings) is null;
    }

    /// <summary>Q8: stops a booking that would overlap a booking still holding this room.</summary>
    /// <exception cref="BusinessRuleException">The room is booked for part of the requested dates; the message gives the dates of that booking.</exception>
    public void EnsureAvailable(DateTime checkInDate, DateTime checkOutDate, IEnumerable<Booking> bookings)
    {
        if (FindConflict(checkInDate, checkOutDate, bookings) is { } conflict)
        {
            throw new BusinessRuleException(
                $"Phòng {Number} đã có đặt phòng từ {conflict.CheckInDate:dd/MM/yyyy} đến {conflict.CheckOutDate:dd/MM/yyyy}.");
        }
    }

    /// <summary>Returns the room number, so the room can be shown directly in lists.</summary>
    public override string ToString()
    {
        return Number;
    }

    /// <summary>
    /// First blocking booking of this room that overlaps the range, or null. Two ranges overlap when each
    /// starts before the other ends. A range whose check-out is not after its check-in overlaps nothing,
    /// so that date error is reported by the <see cref="Booking"/> constructor, not as a double booking.
    /// </summary>
    private Booking? FindConflict(DateTime checkInDate, DateTime checkOutDate, IEnumerable<Booking> bookings)
    {
        DateTime start = checkInDate.Date;
        DateTime end = checkOutDate.Date;

        return bookings.FirstOrDefault(b => b.RoomId == Id
            && b.BlocksRoom
            && start < b.CheckOutDate
            && b.CheckInDate < end);
    }
}
