using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Bookings;

/// <summary>
/// UC7: book a room, check a guest in or out and cancel a reservation. Content is added at stage 3.
/// </summary>
public partial class BookingForm : Form
{
    private readonly BookingService _bookingService;
    private readonly CustomerService _customerService;

    /// <summary>Creates the form.</summary>
    /// <param name="bookingService">Lists bookings and available rooms and performs the booking actions.</param>
    /// <param name="customerService">Finds the customer who books.</param>
    public BookingForm(BookingService bookingService, CustomerService customerService)
    {
        _bookingService = bookingService;
        _customerService = customerService;
        InitializeComponent();
    }
}
