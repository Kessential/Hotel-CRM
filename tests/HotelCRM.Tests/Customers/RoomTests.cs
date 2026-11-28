using HotelCRM.Domain.Common;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>TC2-03: Q8 no double booking, with half-open ranges [check-in, check-out).</summary>
public class RoomTests
{
    private static readonly DateTime Dec10 = new(2026, 12, 10);
    private static readonly DateTime Dec11 = new(2026, 12, 11);
    private static readonly DateTime Dec12 = new(2026, 12, 12);
    private static readonly DateTime Dec13 = new(2026, 12, 13);
    private static readonly DateTime Dec14 = new(2026, 12, 14);

    [Fact]
    public void EnsureAvailable_OverlappingDates_ThrowsBusinessRule()
    {
        // Arrange: room 305 is booked 10–12.
        Room room = CustomerTestData.NewRoom();
        Booking existing = CustomerTestData.NewBooking(room, Dec10, Dec12);

        // Act + Assert: 11–13 shares the night of the 11th.
        Assert.Throws<BusinessRuleException>(() => room.EnsureAvailable(Dec11, Dec13, new[] { existing }));
    }

    [Fact]
    public void EnsureAvailable_StartingOnCheckOutDay_DoesNotThrow()
    {
        // Arrange: room 305 is booked 10–12.
        Room room = CustomerTestData.NewRoom();
        Booking existing = CustomerTestData.NewBooking(room, Dec10, Dec12);

        // Act: 12–14 starts the day the other guest leaves.
        room.EnsureAvailable(Dec12, Dec14, new[] { existing });

        // Assert
        Assert.True(room.IsAvailable(Dec12, Dec14, new[] { existing }));
    }

    [Fact]
    public void EnsureAvailable_SameDatesAfterCancellation_DoesNotThrow()
    {
        // Arrange: the 10–12 booking was cancelled, so it no longer holds the room.
        Room room = CustomerTestData.NewRoom();
        Booking cancelled = CustomerTestData.NewBooking(room, Dec10, Dec12);
        cancelled.Cancel();

        // Act + Assert
        room.EnsureAvailable(Dec10, Dec12, new[] { cancelled });
        Assert.True(room.IsAvailable(Dec10, Dec12, new[] { cancelled }));
    }

    [Fact]
    public void EnsureAvailable_OtherRoomBooked_DoesNotThrow()
    {
        // Arrange: room 306 (same type) is booked 10–12; room 305 is not.
        Room room305 = CustomerTestData.NewRoom();
        Room room306 = new(2, "306", 3, room305.RoomType);
        Booking other = CustomerTestData.NewBooking(room306, Dec10, Dec12);

        // Act + Assert
        room305.EnsureAvailable(Dec10, Dec12, new[] { other });
    }
}
