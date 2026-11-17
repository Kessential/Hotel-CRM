using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>Rule 7: the check-out date must be after the check-in date.</summary>
public class BookingTests
{
    [Fact]
    public void Constructor_CheckOutNotAfterCheckIn_ThrowsValidation()
    {
        // Arrange: a stay that would end on the day it starts.
        Customer customer = CustomerTestData.NewCustomer();
        Room room = CustomerTestData.NewRoom();
        DateTime sameDay = new(2026, 12, 10);

        // Act + Assert
        Assert.Throws<ValidationException>(() => new Booking(1, customer, room, sameDay, sameDay));
    }
}
