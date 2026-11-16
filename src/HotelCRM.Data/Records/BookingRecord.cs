using HotelCRM.Domain.Hotel;

namespace HotelCRM.Data.Records;

/// <summary>One entry of <c>bookings.json</c>. Customer and room are stored by Id.</summary>
public sealed class BookingRecord
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public BookingStatus Status { get; set; }
}
