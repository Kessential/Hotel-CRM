using HotelCRM.Data.Records;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Data.Mappers;

/// <summary>Converts between <see cref="Booking"/> and <see cref="BookingRecord"/>.</summary>
public sealed class BookingMapper : IMapper<Booking, BookingRecord>
{
    private readonly IRepository<Customer> _customers;
    private readonly IRepository<Room> _rooms;

    /// <summary>Creates the mapper.</summary>
    /// <param name="customers">Already loaded customers, used to resolve <c>CustomerId</c>.</param>
    /// <param name="rooms">Already loaded rooms, used to resolve <c>RoomId</c>.</param>
    public BookingMapper(IRepository<Customer> customers, IRepository<Room> rooms)
    {
        _customers = customers;
        _rooms = rooms;
    }

    /// <inheritdoc />
    public BookingRecord ToRecord(Booking entity)
    {
        return new BookingRecord
        {
            Id = entity.Id,
            CustomerId = entity.Customer.Id,
            RoomId = entity.Room.Id,
            CheckInDate = entity.CheckInDate,
            CheckOutDate = entity.CheckOutDate,
            Status = entity.Status,
        };
    }

    /// <inheritdoc />
    public Booking ToEntity(BookingRecord record)
    {
        Customer customer = _customers.GetById(record.CustomerId)
            ?? throw new InvalidDataException($"Booking {record.Id} refers to unknown customer {record.CustomerId}.");
        Room room = _rooms.GetById(record.RoomId)
            ?? throw new InvalidDataException($"Booking {record.Id} refers to unknown room {record.RoomId}.");

        return Booking.Restore(record.Id, customer, room, record.CheckInDate, record.CheckOutDate, record.Status);
    }
}
