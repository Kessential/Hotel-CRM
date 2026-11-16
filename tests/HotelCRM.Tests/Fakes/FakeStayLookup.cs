using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Tests.Fakes;

/// <summary>
/// Hand-written stand-in for the booking subsystem. Lets the ticket tests run without any class of
/// <c>Domain/Hotel</c>: an active stay is just a pair of customer Id and room Id.
/// </summary>
internal sealed class FakeStayLookup : IStayLookup
{
    private readonly HashSet<(int CustomerId, int RoomId)> _activeStays = new();

    /// <summary>Declares that the customer is currently staying in the room.</summary>
    public void AddActiveStay(int customerId, int roomId)
    {
        _activeStays.Add((customerId, roomId));
    }

    public bool IsRoomInActiveStay(int customerId, int roomId)
    {
        return _activeStays.Contains((customerId, roomId));
    }

    public IReadOnlyDictionary<int, string> GetActiveRooms(int customerId)
    {
        return _activeStays
            .Where(stay => stay.CustomerId == customerId)
            .ToDictionary(stay => stay.RoomId, stay => GetRoomNumber(stay.RoomId));
    }

    public string GetRoomNumber(int roomId)
    {
        return roomId.ToString();
    }
}
