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
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<int, string> GetActiveRooms(int customerId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string GetRoomNumber(int roomId)
    {
        throw new NotImplementedException();
    }
}
