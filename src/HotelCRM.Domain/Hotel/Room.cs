using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>A physical room of the hotel. Decides whether it is free for a date range (rule 7).</summary>
public sealed class Room : IEntity
{
    /// <summary>Creates a room and registers it in its room type.</summary>
    /// <param name="id">Id obtained from the room repository.</param>
    /// <param name="number">Room number shown to staff and guests, for example "305"; required.</param>
    /// <param name="floor">Floor the room is on; must be greater than 0.</param>
    /// <param name="roomType">Category and price of the room; required.</param>
    /// <exception cref="ValidationException">The number is empty, the floor is not positive, or the room type is missing.</exception>
    public Room(int id, string number, int floor, RoomType roomType)
        : this(id, number, floor, roomType, validate: true)
    {
    }

    private Room(int id, string number, int floor, RoomType roomType, bool validate)
    {
        Id = id;
        Number = number;
        Floor = floor;
        RoomType = roomType;

        if (validate)
        {
            Validate();
        }

        roomType.AddRoom(this);
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Room number; never empty.</summary>
    public string Number { get; }

    /// <summary>Floor the room is on.</summary>
    public int Floor { get; }

    /// <summary>Category and price of the room.</summary>
    public RoomType RoomType { get; }

    /// <summary>
    /// Rule 7: whether the room is free from <paramref name="checkInDate"/> up to, but not including,
    /// <paramref name="checkOutDate"/>. Only bookings of this room that block it
    /// (<see cref="Booking.BlocksRoom"/>: Reserved or CheckedIn) are considered. Two stays overlap when
    /// each starts before the other ends, so a guest may check in on the day another one checks out.
    /// </summary>
    /// <param name="checkInDate">First night of the requested stay.</param>
    /// <param name="checkOutDate">Departure day of the requested stay.</param>
    /// <param name="bookings">Existing bookings; bookings of other rooms are ignored.</param>
    public bool IsAvailable(DateTime checkInDate, DateTime checkOutDate, IEnumerable<Booking> bookings)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 7: stops a booking when <see cref="IsAvailable"/> returns false.</summary>
    /// <exception cref="BusinessRuleException">The room is already booked for part of the requested dates.</exception>
    public void EnsureAvailable(DateTime checkInDate, DateTime checkOutDate, IEnumerable<Booking> bookings)
    {
        throw new NotImplementedException();
    }

    /// <summary>Returns the room number, so the room can be shown directly in lists.</summary>
    public override string ToString()
    {
        return Number;
    }

    /// <summary>Rebuilds a room from stored data.</summary>
    internal static Room Restore(int id, string number, int floor, RoomType roomType)
    {
        return new Room(id, number, floor, roomType, validate: false);
    }

    /// <summary>Throws <see cref="ValidationException"/> when the number is empty, the floor is not positive or the room type is missing.</summary>
    private void Validate()
    {
        throw new NotImplementedException();
    }
}
