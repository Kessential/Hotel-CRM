using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Business.Services;

/// <summary>UC7: book rooms, check guests in and out, cancel reservations.</summary>
public sealed class BookingService
{
    private readonly IRepository<Booking> _bookings;
    private readonly IRepository<Room> _rooms;
    private readonly IRepository<Customer> _customers;

    /// <summary>Creates the service.</summary>
    /// <param name="bookings">Booking repository.</param>
    /// <param name="rooms">Room repository.</param>
    /// <param name="customers">Customer repository; check-in and check-out change the customer.</param>
    public BookingService(IRepository<Booking> bookings, IRepository<Room> rooms, IRepository<Customer> customers)
    {
        _bookings = bookings;
        _rooms = rooms;
        _customers = customers;
    }

    /// <summary>
    /// UC7: reserves a room. Steps: find customer and room; <see cref="Room.EnsureAvailable"/> with all
    /// bookings (rule 7); take the next Id; create the <see cref="Booking"/> (date check of rule 7); save.
    /// </summary>
    /// <returns>The saved booking, status Reserved.</returns>
    /// <exception cref="ValidationException">The check-out date is not after the check-in date.</exception>
    /// <exception cref="BusinessRuleException">Customer or room does not exist, or the room is already booked for part of the dates.</exception>
    public Booking Book(int customerId, int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        Customer customer = _customers.GetRequired(customerId);
        Room room = _rooms.GetRequired(roomId);
        room.EnsureAvailable(checkInDate, checkOutDate, _bookings.GetAll());
        Booking booking = new(_bookings.NextId(), customer, room, checkInDate, checkOutDate);
        _bookings.Save(booking);
        return booking;
    }

    /// <summary>
    /// UC7: checks the guest in. Steps: <see cref="Booking.CheckIn"/> with <paramref name="now"/>; record a
    /// FrontDesk interaction on the customer with <see cref="Customer.LogInteraction"/>; save booking and customer.
    /// </summary>
    /// <param name="bookingId">Id of the booking.</param>
    /// <param name="now">Current time, supplied by the form; checked against the stay dates and used for the interaction.</param>
    /// <exception cref="BusinessRuleException">
    /// The booking does not exist, is not Reserved, or the date of <paramref name="now"/> is outside its stay.
    /// </exception>
    public void CheckIn(int bookingId, DateTime now)
    {
        Booking booking = _bookings.GetRequired(bookingId);
        booking.CheckIn(now);
        booking.Customer.LogInteraction(
            ContactChannel.FrontDesk, $"Checked in to room {booking.Room.Number}.", now, null);
        _bookings.Save(booking);
        _customers.Save(booking.Customer);
    }

    /// <summary>
    /// UC7: checks the guest out. Steps: <see cref="Booking.CheckOut"/>; add <see cref="Booking.PointsEarned"/>
    /// with <see cref="Customer.AddPoints"/> (rule 8); record a FrontDesk interaction; save booking and customer.
    /// </summary>
    /// <param name="bookingId">Id of the booking.</param>
    /// <param name="now">Current time, supplied by the form; used for the interaction.</param>
    /// <exception cref="BusinessRuleException">The booking does not exist or is not CheckedIn.</exception>
    public void CheckOut(int bookingId, DateTime now)
    {
        Booking booking = _bookings.GetRequired(bookingId);
        booking.CheckOut();
        int points = booking.PointsEarned;
        if (points > 0)
        {
            booking.Customer.AddPoints(points);
        }

        booking.Customer.LogInteraction(
            ContactChannel.FrontDesk,
            $"Checked out of room {booking.Room.Number}; earned {points} loyalty points.",
            now,
            null);
        _bookings.Save(booking);
        _customers.Save(booking.Customer);
    }

    /// <summary>UC7: cancels a reservation. Steps: <see cref="Booking.Cancel"/> (rule 7); save.</summary>
    /// <exception cref="BusinessRuleException">The booking does not exist or is not Reserved.</exception>
    public void Cancel(int bookingId)
    {
        Booking booking = _bookings.GetRequired(bookingId);
        booking.Cancel();
        _bookings.Save(booking);
    }

    /// <summary>Returns every booking, newest check-in date first.</summary>
    public IReadOnlyList<Booking> GetAll()
    {
        return _bookings.GetAll()
            .OrderByDescending(b => b.CheckInDate)
            .ThenByDescending(b => b.Id)
            .ToList();
    }

    /// <summary>
    /// Returns the rooms for which <see cref="Room.IsAvailable"/> is true for the given dates, ordered
    /// by floor, then by room number (shorter numbers first, so "99" comes before "305"). Returns an empty list when the check-out date is not after the check-in date.
    /// </summary>
    public IReadOnlyList<Room> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate)
    {
        if (checkOutDate.Date <= checkInDate.Date)
        {
            return Array.Empty<Room>();
        }

        IReadOnlyList<Booking> bookings = _bookings.GetAll();
        return _rooms.GetAll()
            .Where(r => r.IsAvailable(checkInDate, checkOutDate, bookings))
            .OrderBy(r => r.Floor)
            .ThenBy(r => r.Number.Length)
            .ThenBy(r => r.Number, StringComparer.Ordinal)
            .ToList();
    }
}
