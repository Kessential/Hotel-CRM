using HotelCRM.Data.Records;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Data.Mappers;

/// <summary>Converts between <see cref="RoomType"/> and <see cref="RoomTypeRecord"/>.</summary>
public sealed class RoomTypeMapper : IMapper<RoomType, RoomTypeRecord>
{
    /// <inheritdoc />
    public RoomTypeRecord ToRecord(RoomType entity)
    {
        return new RoomTypeRecord
        {
            Id = entity.Id,
            Name = entity.Name,
            PricePerNight = entity.PricePerNight,
            Capacity = entity.Capacity,
        };
    }

    /// <inheritdoc />
    public RoomType ToEntity(RoomTypeRecord record)
    {
        return RoomType.Restore(record.Id, record.Name, record.PricePerNight, record.Capacity);
    }
}
