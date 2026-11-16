using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>
/// A stay of one customer in one room. Holds the customer and the room as objects because all three
/// belong to the same subsystem. Enforces the date rule and the status changes of rule 7.
/// </summary>
public sealed class Booking : IEntity
{
    /// <summary>Creates a new booking with status Reserved.</summary>
    /// <param name="id">Id obtained from the booking repository.</param>
    /// <param name="customer">Guest; required.</param>
    /// <param name="room">Room; required. The service checks its availability with <see cref="Room.EnsureAvailable"/> first.</param>
    /// <param name="checkInDate">Arrival day; the time of day is discarded.</param>
    /// <param name="checkOutDate">Departure day; the time of day is discarded. Must be after <paramref name="checkInDate"/>.</param>
    /// <exception cref="ValidationException">
    /// Customer or room is missing, or the check-out date is not after the check-in date (rule 7).
    /// </exception>
    public Booking(int id, Customer customer, Room room, DateTime checkInDate, DateTime checkOutDate)
        : this(id, customer, room, checkInDate.Date, checkOutDate.Date, BookingStatus.Reserved)
    {
        ValidateNewBooking();
    }

    private Booking(int id, Customer customer, Room room, DateTime checkInDate, DateTime checkOutDate, BookingStatus status)
    {
        Id = id;
        Customer = customer;
        Room = room;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Status = status;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Guest who made the booking.</summary>
    public Customer Customer { get; }

    /// <summary>Booked room.</summary>
    public Room Room { get; }

    /// <summary>Arrival day (midnight).</summary>
    public DateTime CheckInDate { get; }

    /// <summary>Departure day (midnight); always after <see cref="CheckInDate"/>.</summary>
    public DateTime CheckOutDate { get; }

    /// <summary>Current status; changed only by <see cref="CheckIn"/>, <see cref="CheckOut"/> and <see cref="Cancel"/>.</summary>
    public BookingStatus Status { get; private set; }

    /// <summary>Number of nights: days between <see cref="CheckInDate"/> and <see cref="CheckOutDate"/>.</summary>
    public int Nights => throw new NotImplementedException();

    /// <summary>Price of the stay in USD: <see cref="Nights"/> x <see cref="RoomType.PricePerNight"/> of the room.</summary>
    public decimal TotalAmount => throw new NotImplementedException();

    /// <summary>Rule 8: loyalty points awarded at check-out, 1 point per whole USD of <see cref="TotalAmount"/>.</summary>
    public int PointsEarned => throw new NotImplementedException();

    /// <summary>Rule 7: whether this booking keeps its room occupied: true for Reserved and CheckedIn.</summary>
    public bool BlocksRoom => throw new NotImplementedException();

    /// <summary>Reserved to CheckedIn: the guest arrives and the stay becomes active.</summary>
    /// <exception cref="BusinessRuleException">The booking is not Reserved.</exception>
    public void CheckIn()
    {
        throw new NotImplementedException();
    }

    /// <summary>CheckedIn to CheckedOut: the guest leaves. The service then awards <see cref="PointsEarned"/>.</summary>
    /// <exception cref="BusinessRuleException">The booking is not CheckedIn.</exception>
    public void CheckOut()
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 7: Reserved to Cancelled. A booking that was checked in or out cannot be cancelled.</summary>
    /// <exception cref="BusinessRuleException">The booking is not Reserved.</exception>
    public void Cancel()
    {
        throw new NotImplementedException();
    }

    /// <summary>Rebuilds a booking from stored data, including its status.</summary>
    internal static Booking Restore(
        int id,
        Customer customer,
        Room room,
        DateTime checkInDate,
        DateTime checkOutDate,
        BookingStatus status)
    {
        return new Booking(id, customer, room, checkInDate, checkOutDate, status);
    }

    /// <summary>Throws <see cref="ValidationException"/> when customer or room is missing or the dates are not in order.</summary>
    private void ValidateNewBooking()
    {
        throw new NotImplementedException();
    }
}
