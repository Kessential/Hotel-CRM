using HotelCRM.Domain.Common;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>TC2-04 (Q8 check-in window, boundary times) and the other Q8 rules of a single booking.</summary>
public class BookingTests
{
    private static readonly DateTime Dec10 = new(2026, 12, 10);
    private static readonly DateTime Dec12 = new(2026, 12, 12);

    [Fact]
    public void Constructor_CheckOutSameDayAsCheckIn_ThrowsValidation()
    {
        Room room = CustomerTestData.NewRoom();

        Assert.Throws<ValidationException>(() => CustomerTestData.NewBooking(room, Dec10, Dec10.AddHours(20)));
    }

    [Fact]
    public void Constructor_TwoNights_FixesRateAndTotal()
    {
        // Arrange: 10/12 to 12/12 = 2 nights at 1.250.000đ.
        Room room = CustomerTestData.NewRoom();

        // Act
        Booking booking = CustomerTestData.NewBooking(room, Dec10, Dec12);

        // Assert: 2 x 1.250.000 = 2.500.000đ.
        Assert.Equal(BookingStatus.Reserved, booking.Status);
        Assert.Equal(2, booking.Nights);
        Assert.Equal(1_250_000m, booking.RatePerNight);
        Assert.Equal(2_500_000m, booking.TotalAmount);
        Assert.Equal(room.Id, booking.RoomId);
    }

    // ---- TC2-04: check-in allowed only in [check-in day, check-out day) ----

    [Fact]
    public void CheckIn_AtMidnightOfCheckInDay_BecomesCheckedIn()
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);
        DateTime now = new(2026, 12, 10, 0, 0, 0);

        booking.CheckIn(now);

        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
        Assert.Equal(now, booking.CheckedInAt);
    }

    [Theory]
    [InlineData(2026, 12, 9, 23, 59)] // one minute before the check-in day
    [InlineData(2026, 12, 12, 0, 0)]  // the check-out day itself
    public void CheckIn_OutsideStay_ThrowsBusinessRuleAndKeepsReserved(int year, int month, int day, int hour, int minute)
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);

        Assert.Throws<BusinessRuleException>(() => booking.CheckIn(new DateTime(year, month, day, hour, minute, 0)));

        Assert.Equal(BookingStatus.Reserved, booking.Status);
        Assert.Null(booking.CheckedInAt);
    }

    // ---- Q8 status changes ----

    [Fact]
    public void CheckOut_WhenCheckedIn_BecomesCheckedOut()
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);
        booking.CheckIn(Dec10.AddHours(14));
        DateTime leaving = new(2026, 12, 11, 10, 0, 0); // one day early

        booking.CheckOut(leaving);

        Assert.Equal(BookingStatus.CheckedOut, booking.Status);
        Assert.Equal(leaving, booking.CheckedOutAt);
        Assert.Equal(2_500_000m, booking.TotalAmount); // early check-out still pays both nights
    }

    [Fact]
    public void CheckOut_WhenReserved_ThrowsBusinessRule()
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);

        Assert.Throws<BusinessRuleException>(() => booking.CheckOut(Dec10.AddHours(14)));
    }

    [Fact]
    public void Cancel_WhenCheckedIn_ThrowsBusinessRule()
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);
        booking.CheckIn(Dec10.AddHours(14));

        Assert.Throws<BusinessRuleException>(() => booking.Cancel());
        Assert.Equal(BookingStatus.CheckedIn, booking.Status);
    }

    [Fact]
    public void CheckIn_WhenCancelled_ThrowsBusinessRule()
    {
        Booking booking = CustomerTestData.NewBooking(CustomerTestData.NewRoom(), Dec10, Dec12);
        booking.Cancel();

        Assert.Throws<BusinessRuleException>(() => booking.CheckIn(Dec10.AddHours(14)));
    }
}
