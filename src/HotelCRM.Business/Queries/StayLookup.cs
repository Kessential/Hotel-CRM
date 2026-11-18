using HotelCRM.Domain.Hotel;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Business.Queries;

/// <summary>
/// Answers the ticket subsystem's questions about stays. Built on the booking and room repositories
/// only (not on <c>BookingService</c>). An active stay is a booking with status CheckedIn.
/// </summary>
public sealed class StayLookup : IStayLookup
{
    private readonly IRepository<Booking> _bookings;
    private readonly IRepository<Room> _rooms;

    /// <summary>Creates the lookup.</summary>
    /// <param name="bookings">Booking repository.</param>
    /// <param name="rooms">Room repository.</param>
    public StayLookup(IRepository<Booking> bookings, IRepository<Room> rooms)
    {
        _bookings = bookings;
        _rooms = rooms;
    }

    /// <inheritdoc />
    public bool IsRoomInActiveStay(int customerId, int roomId)
    {
        return ActiveStays(customerId).Any(b => b.Room.Id == roomId);
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<int, string> GetActiveRooms(int customerId)
    {
        Dictionary<int, string> rooms = new();
        foreach (Booking booking in ActiveStays(customerId))
        {
            rooms[booking.Room.Id] = booking.Room.Number;
        }

        return rooms;
    }

    /// <inheritdoc />
    public string GetRoomNumber(int roomId)
    {
        return _rooms.GetById(roomId)?.Number ?? string.Empty;
    }

    private IEnumerable<Booking> ActiveStays(int customerId)
    {
        return _bookings.GetAll()
            .Where(b => b.Customer.Id == customerId && b.Status == BookingStatus.CheckedIn);
    }
}
