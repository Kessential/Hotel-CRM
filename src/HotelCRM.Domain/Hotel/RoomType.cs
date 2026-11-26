using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>
/// A category of rooms with one current price. Aggregates the rooms of that category: rooms register
/// themselves here when they are created.
/// </summary>
public sealed class RoomType : IEntity
{
    private readonly List<Room> _rooms = new();

    /// <summary>Creates a room type.</summary>
    /// <param name="id">Id assigned by the room type repository.</param>
    /// <param name="name">Display name, for example "Deluxe đôi"; required.</param>
    /// <param name="pricePerNight">Current price of one night in VND; must be greater than 0.</param>
    /// <param name="capacity">Number of guests the room sleeps; must be greater than 0.</param>
    /// <exception cref="ValidationException">The name is empty, or price or capacity is not positive.</exception>
    public RoomType(int id, string name, decimal pricePerNight, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tên loại phòng không được để trống.");
        }

        if (pricePerNight <= 0)
        {
            throw new ValidationException("Giá mỗi đêm phải lớn hơn 0.");
        }

        if (capacity <= 0)
        {
            throw new ValidationException("Sức chứa phải lớn hơn 0.");
        }

        Id = id;
        Name = name.Trim();
        PricePerNight = pricePerNight;
        Capacity = capacity;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Display name; never empty.</summary>
    public string Name { get; }

    /// <summary>Current price of one night in VND. A booking copies it into <see cref="Booking.RatePerNight"/>.</summary>
    public decimal PricePerNight { get; }

    /// <summary>Number of guests the room sleeps; greater than 0.</summary>
    public int Capacity { get; }

    /// <summary>Rooms of this type.</summary>
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    /// <summary>Returns the display name, so the room type can be shown directly in lists.</summary>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>Called by <see cref="Room"/> when a room of this type is created.</summary>
    internal void AddRoom(Room room)
    {
        if (!_rooms.Contains(room))
        {
            _rooms.Add(room);
        }
    }
}
