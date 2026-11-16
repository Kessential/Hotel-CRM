using HotelCRM.Data.Records;
using HotelCRM.Domain.Hotel;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Data.Mappers;

/// <summary>Converts between <see cref="Room"/> and <see cref="RoomRecord"/>.</summary>
public sealed class RoomMapper : IMapper<Room, RoomRecord>
{
    private readonly IRepository<RoomType> _roomTypes;

    /// <summary>Creates the mapper.</summary>
    /// <param name="roomTypes">Already loaded room types, used to resolve <c>RoomTypeId</c>.</param>
    public RoomMapper(IRepository<RoomType> roomTypes)
    {
        _roomTypes = roomTypes;
    }

    /// <inheritdoc />
    public RoomRecord ToRecord(Room entity)
    {
        return new RoomRecord
        {
            Id = entity.Id,
            Number = entity.Number,
            Floor = entity.Floor,
            RoomTypeId = entity.RoomType.Id,
        };
    }

    /// <inheritdoc />
    public Room ToEntity(RoomRecord record)
    {
        RoomType roomType = _roomTypes.GetById(record.RoomTypeId)
            ?? throw new InvalidDataException($"Room {record.Id} refers to unknown room type {record.RoomTypeId}.");

        return Room.Restore(record.Id, record.Number, record.Floor, roomType);
    }
}
