using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.Tests.Customers;

/// <summary>Fixed values and small builders shared by the customer, booking and survey tests.</summary>
internal static class CustomerTestData
{
    /// <summary>The fixed "now" of every test in this folder.</summary>
    public static readonly DateTime Now = new(2026, 11, 16, 9, 0, 0);

    public const string ValidPhone = "0912345678";
    public const string ValidEmail = "nguyen.an@example.com";
    public const string ValidCitizenId = "001203004567";

    /// <summary>A valid new Vietnamese customer: Standard tier, 0 lifetime points.</summary>
    public static Customer NewCustomer(
        int id = 1,
        string phone = ValidPhone,
        string email = ValidEmail,
        string identityNumber = ValidCitizenId,
        string nationality = Customer.VietnameseNationality,
        DateTime? dateOfBirth = null)
    {
        return new Customer(
            id, "Nguyễn Văn An", phone, email, identityNumber, nationality, dateOfBirth,
            marketingConsent: true, "Thích phòng yên tĩnh.", Now);
    }

    /// <summary>Id of the receptionist who makes the bookings in the tests.</summary>
    public const int ReceptionistId = 3;

    /// <summary>Room 305, a "Deluxe đôi" at 1.250.000đ per night.</summary>
    public static Room NewRoom()
    {
        RoomType deluxe = new(1, "Deluxe đôi", 1_250_000m, 2);
        return new Room(1, "305", 3, deluxe);
    }

    /// <summary>A Reserved booking of <paramref name="room"/> for customer 1, made at <see cref="Now"/>.</summary>
    public static Booking NewBooking(Room room, DateTime checkInDate, DateTime checkOutDate, int id = 1)
    {
        return new Booking(id, customerId: 1, room, checkInDate, checkOutDate, ReceptionistId, Now);
    }
}
