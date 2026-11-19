using HotelCRM.Business.Services;
using HotelCRM.Domain.Customers;
using HotelCRM.Domain.Hotel;

namespace HotelCRM.WinForms.Forms.Bookings;

/// <summary>
/// UC7: book a room, check a guest in or out and cancel a reservation. The top row finds the customer
/// and the free rooms for the chosen dates; the grid lists every booking.
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
        dgvBookings.AutoGenerateColumns = false;
    }

    private void BookingForm_Load(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            dtpCheckIn.Value = DateTime.Today;
            dtpCheckOut.Value = DateTime.Today.AddDays(1);
            LoadBookings();
        });
    }

    private void btnFindCustomer_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            cboCustomer.Items.Clear();
            foreach (Customer customer in _customerService.Search(txtCustomerKeyword.Text))
            {
                cboCustomer.Items.Add(new CustomerOption(customer.Id, $"{customer.FullName} ({customer.Phone})"));
            }

            if (cboCustomer.Items.Count > 0)
            {
                cboCustomer.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No customer matches the keyword.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void btnFindRooms_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            LoadAvailableRooms();
            if (cboRoom.Items.Count == 0)
            {
                MessageBox.Show(
                    "No room is free for the chosen dates. Check that the check-out date is after the check-in date.",
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        });
    }

    private void btnBook_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (cboCustomer.SelectedItem is not CustomerOption customer)
            {
                MessageBox.Show("Find and select a customer first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboRoom.SelectedItem is not RoomOption room)
            {
                MessageBox.Show("Find and select a room first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // The room list is not refreshed here: it shows the result of the last search, and the
            // service checks the room again on every booking.
            Booking booking = _bookingService.Book(customer.Id, room.Id, dtpCheckIn.Value.Date, dtpCheckOut.Value.Date);
            LoadBookings();
            MessageBox.Show(
                $"Booking {booking.Id} created: room {booking.Room.Number}, {booking.Nights} night(s), {booking.TotalAmount:0.00} USD.",
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
    }

    private void btnCheckIn_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (SelectedRow() is BookingRow row)
            {
                _bookingService.CheckIn(row.Id, DateTime.Now);
                LoadBookings();
            }
        });
    }

    private void btnCheckOut_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (SelectedRow() is BookingRow row)
            {
                _bookingService.CheckOut(row.Id, DateTime.Now);
                LoadBookings();
            }
        });
    }

    private void btnCancelBooking_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            BookingRow? row = SelectedRow();
            if (row is null)
            {
                return;
            }

            DialogResult answer = MessageBox.Show(
                $"Cancel booking {row.Id} of {row.Customer}?",
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (answer == DialogResult.Yes)
            {
                _bookingService.Cancel(row.Id);
                LoadBookings();
            }
        });
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(Close);
    }

    /// <summary>Fills the room list with the rooms that are free for the chosen dates; empty for invalid dates.</summary>
    private void LoadAvailableRooms()
    {
        cboRoom.Items.Clear();
        foreach (Room room in _bookingService.GetAvailableRooms(dtpCheckIn.Value.Date, dtpCheckOut.Value.Date))
        {
            cboRoom.Items.Add(new RoomOption(
                room.Id,
                $"{room.Number} - {room.RoomType.Name} - {room.RoomType.PricePerNight:0.##} USD/night"));
        }

        if (cboRoom.Items.Count > 0)
        {
            cboRoom.SelectedIndex = 0;
        }
    }

    /// <summary>Fills the grid with every booking.</summary>
    private void LoadBookings()
    {
        dgvBookings.DataSource = _bookingService.GetAll()
            .Select(BookingRow.From)
            .ToList();
    }

    /// <summary>Returns the selected booking row, or tells the user to select one and returns null.</summary>
    private BookingRow? SelectedRow()
    {
        if (dgvBookings.CurrentRow?.DataBoundItem is BookingRow row)
        {
            return row;
        }

        MessageBox.Show("Select a booking first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    /// <summary>An entry of the customer list.</summary>
    private sealed record CustomerOption(int Id, string Display)
    {
        public override string ToString()
        {
            return Display;
        }
    }

    /// <summary>An entry of the room list.</summary>
    private sealed record RoomOption(int Id, string Display)
    {
        public override string ToString()
        {
            return Display;
        }
    }

    /// <summary>One line of the booking grid.</summary>
    private sealed record BookingRow(
        int Id,
        string Customer,
        string Room,
        string RoomType,
        string CheckIn,
        string CheckOut,
        int Nights,
        string Total,
        string Status)
    {
        public static BookingRow From(Booking booking)
        {
            return new BookingRow(
                booking.Id,
                booking.Customer.FullName,
                booking.Room.Number,
                booking.Room.RoomType.Name,
                booking.CheckInDate.ToString("yyyy-MM-dd"),
                booking.CheckOutDate.ToString("yyyy-MM-dd"),
                booking.Nights,
                booking.TotalAmount.ToString("0.00"),
                booking.Status.ToString());
        }
    }
}
