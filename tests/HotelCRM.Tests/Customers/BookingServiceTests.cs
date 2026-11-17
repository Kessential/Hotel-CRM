using HotelCRM.Business.Services;
using HotelCRM.Data;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>
/// Rule 7: a room cannot be booked twice for overlapping dates, checked through
/// <see cref="BookingService"/> with in-memory repositories.
/// </summary>
public class BookingServiceTests
{
    [Fact]
    public void Book_OverlappingDates_ThrowsBusinessRule()
    {
        // Arrange: room 305 is reserved from 10 to 13 December.
        InMemoryRepository<Booking> bookings = new();
        InMemoryRepository<Room> rooms = new();
        InMemoryRepository<Customer> customers = new();
        Customer first = CustomerTestData.NewCustomer(1);
        Customer second = CustomerTestData.NewCustomer(2, "0987654321", "liam.brown@example.com");
        Room room = CustomerTestData.NewRoom();
        customers.Save(first);
        customers.Save(second);
        rooms.Save(room);
        BookingService service = new(bookings, rooms, customers);
        service.Book(first.Id, room.Id, new DateTime(2026, 12, 10), new DateTime(2026, 12, 13));

        // Act + Assert: another guest asks for 12 to 14 December, which overlaps by one night.
        Assert.Throws<BusinessRuleException>(
            () => service.Book(second.Id, room.Id, new DateTime(2026, 12, 12), new DateTime(2026, 12, 14)));
    }
}
