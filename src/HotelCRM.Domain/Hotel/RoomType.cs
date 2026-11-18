using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>
/// A category of rooms with one price. Aggregates the rooms of that category: rooms register
/// themselves here when they are created.
/// </summary>
public sealed class RoomType : IEntity
{
    private readonly List<Room> _rooms = new();

    /// <summary>Creates a room type.</summary>
    /// <param name="id">Id obtained from the room type repository.</param>
    /// <param name="name">Display name, for example "Deluxe Double"; required.</param>
    /// <param name="pricePerNight">Price of one night in USD; must be greater than 0.</param>
    /// <param name="capacity">Number of guests the room sleeps; must be greater than 0.</param>
    /// <exception cref="ValidationException">The name is empty, or price or capacity is not positive.</exception>
    public RoomType(int id, string name, decimal pricePerNight, int capacity)
        : this(id, name, pricePerNight, capacity, validate: true)
    {
    }

    private RoomType(int id, string name, decimal pricePerNight, int capacity, bool validate)
    {
        Id = id;
        Name = name;
        PricePerNight = pricePerNight;
        Capacity = capacity;

        if (validate)
        {
            Validate();
        }
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Display name; never empty.</summary>
    public string Name { get; }

    /// <summary>Price of one night in USD; greater than 0.</summary>
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

    /// <summary>Rebuilds a room type from stored data.</summary>
    internal static RoomType Restore(int id, string name, decimal pricePerNight, int capacity)
    {
        return new RoomType(id, name, pricePerNight, capacity, validate: false);
    }

    /// <summary>Called by <see cref="Room"/> when a room of this type is created.</summary>
    internal void AddRoom(Room room)
    {
        if (!_rooms.Contains(room))
        {
            _rooms.Add(room);
        }
    }

    /// <summary>Throws <see cref="ValidationException"/> when the name is empty or price or capacity is not positive.</summary>
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ValidationException("Room type name is required.");
        }

        if (PricePerNight <= 0)
        {
            throw new ValidationException("Price per night must be greater than 0.");
        }

        if (Capacity <= 0)
        {
            throw new ValidationException("Capacity must be greater than 0.");
        }
    }
}
