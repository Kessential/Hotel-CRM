using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>Fixed values and small builders shared by the customer, booking and survey tests.</summary>
internal static class CustomerTestData
{
    /// <summary>The fixed "now" of every test in this folder.</summary>
    public static readonly DateTime Now = new(2026, 11, 16, 9, 0, 0);

    public const string ValidPhone = "0912345678";
    public const string ValidEmail = "emma.stone@example.com";

    /// <summary>A valid new customer: Standard tier, 0 points.</summary>
    public static Customer NewCustomer(int id = 1, string phone = ValidPhone, string email = ValidEmail)
    {
        return new Customer(id, "Emma Stone", phone, email, "P1234567", "British", "Prefers a quiet room.");
    }

    /// <summary>Room 305, a Deluxe Double at 120 USD per night.</summary>
    public static Room NewRoom()
    {
        RoomType deluxe = new(1, "Deluxe Double", 120m, 2);
        return new Room(1, "305", 3, deluxe);
    }
}
