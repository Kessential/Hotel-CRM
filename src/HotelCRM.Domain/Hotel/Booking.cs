using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Hotel;

/// <summary>
/// One stay of one customer in one room (BM5, UC08). Decides Q8: dates in order, the price per night
/// fixed at booking time, and the status changes Reserved → CheckedIn → CheckedOut or Reserved → Cancelled
/// (state machine in <c>08-hanh-vi.md</c> section 4). Refers to the customer, the room and the employee by Id.
/// </summary>
public sealed class Booking : IEntity
{
    /// <summary>Creates a booking with status Reserved.</summary>
    /// <param name="id">Id assigned by the booking repository.</param>
    /// <param name="customerId">Guest.</param>
    /// <param name="room">
    /// Booked room. Its current <see cref="RoomType.PricePerNight"/> is copied into <see cref="RatePerNight"/>.
    /// The service checks <see cref="Room.EnsureAvailable"/> first.
    /// </param>
    /// <param name="checkInDate">Arrival day; the time of day is discarded.</param>
    /// <param name="checkOutDate">Departure day; the time of day is discarded. Must be after <paramref name="checkInDate"/>.</param>
    /// <param name="createdById">Employee who made the booking.</param>
    /// <param name="now">Current time, from <c>IClock</c>.</param>
    /// <exception cref="ValidationException">The check-out date is not after the check-in date (Q8).</exception>
    public Booking(
        int id,
        int customerId,
        Room room,
        DateTime checkInDate,
        DateTime checkOutDate,
        int createdById,
        DateTime now)
    {
        ArgumentNullException.ThrowIfNull(room);
        if (checkOutDate.Date <= checkInDate.Date)
        {
            throw new ValidationException("Ngày trả phòng phải sau ngày nhận phòng.");
        }

        Id = id;
        CustomerId = customerId;
        RoomId = room.Id;
        CheckInDate = checkInDate.Date;
        CheckOutDate = checkOutDate.Date;
        RatePerNight = room.RoomType.PricePerNight;
        Status = BookingStatus.Reserved;
        CreatedById = createdById;
        CreatedAt = now;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Guest who made the booking.</summary>
    public int CustomerId { get; }

    /// <summary>Booked room.</summary>
    public int RoomId { get; }

    /// <summary>Arrival day (midnight).</summary>
    public DateTime CheckInDate { get; }

    /// <summary>Departure day (midnight); always after <see cref="CheckInDate"/>.</summary>
    public DateTime CheckOutDate { get; }

    /// <summary>
    /// Q8, historical attribute: price of one night in VND when the booking was made. Later price changes
    /// of the room type do not affect it, so old totals, points (Q9) and spending reports stay correct.
    /// </summary>
    public decimal RatePerNight { get; }

    /// <summary>Current status; changed only by <see cref="CheckIn"/>, <see cref="CheckOut"/> and <see cref="Cancel"/>.</summary>
    public BookingStatus Status { get; private set; }

    /// <summary>Employee who made the booking.</summary>
    public int CreatedById { get; }

    /// <summary>When the booking was made.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>When the guest checked in; set exactly when the status is CheckedIn or CheckedOut.</summary>
    public DateTime? CheckedInAt { get; private set; }

    /// <summary>When the guest checked out; set exactly when the status is CheckedOut.</summary>
    public DateTime? CheckedOutAt { get; private set; }

    /// <summary>Number of booked nights. An early check-out still pays every booked night (Q8).</summary>
    public int Nights => (CheckOutDate - CheckInDate).Days;

    /// <summary>Amount of the stay in VND: <see cref="Nights"/> x <see cref="RatePerNight"/>.</summary>
    public decimal TotalAmount => Nights * RatePerNight;

    /// <summary>Q8: whether the booking keeps its room occupied for its dates (Reserved or CheckedIn).</summary>
    public bool BlocksRoom => Status is BookingStatus.Reserved or BookingStatus.CheckedIn;

    /// <summary>
    /// Q8: Reserved → CheckedIn. Allowed only from <see cref="CheckInDate"/> up to the day before
    /// <see cref="CheckOutDate"/>, so the range is half-open: [check-in, check-out).
    /// </summary>
    /// <param name="now">Current time; only its date is compared with the stay.</param>
    /// <exception cref="BusinessRuleException">
    /// The booking is not Reserved, or <paramref name="now"/> is outside [check-in, check-out). The status does not change.
    /// </exception>
    public void CheckIn(DateTime now)
    {
        EnsureStatus(BookingStatus.Reserved, "nhận phòng");

        if (now.Date < CheckInDate || now.Date >= CheckOutDate)
        {
            throw new BusinessRuleException(
                $"Đặt phòng {Id} chỉ nhận phòng được từ {CheckInDate:dd/MM/yyyy} đến trước {CheckOutDate:dd/MM/yyyy}.");
        }

        Status = BookingStatus.CheckedIn;
        CheckedInAt = now;
    }

    /// <summary>
    /// Q8: CheckedIn → CheckedOut. The service then earns the points of <see cref="TotalAmount"/> (Q9)
    /// and schedules the survey (L3) in the same transaction.
    /// </summary>
    /// <param name="now">Current time, stored in <see cref="CheckedOutAt"/>.</param>
    /// <exception cref="BusinessRuleException">The booking is not CheckedIn.</exception>
    public void CheckOut(DateTime now)
    {
        EnsureStatus(BookingStatus.CheckedIn, "trả phòng");

        Status = BookingStatus.CheckedOut;
        CheckedOutAt = now;
    }

    /// <summary>Q8: Reserved → Cancelled. A booking already checked in or out cannot be cancelled.</summary>
    /// <exception cref="BusinessRuleException">The booking is not Reserved.</exception>
    public void Cancel()
    {
        EnsureStatus(BookingStatus.Reserved, "hủy");

        Status = BookingStatus.Cancelled;
    }

    /// <summary>Throws when the booking is not in the status an action starts from.</summary>
    private void EnsureStatus(BookingStatus required, string action)
    {
        if (Status != required)
        {
            throw new BusinessRuleException(
                $"Đặt phòng {Id} đang ở trạng thái {Describe(Status)}, không thể {action}.");
        }
    }

    /// <summary>Vietnamese name of a status for error messages.</summary>
    private static string Describe(BookingStatus status)
    {
        return status switch
        {
            BookingStatus.Reserved => "Đã đặt",
            BookingStatus.CheckedIn => "Đang ở",
            BookingStatus.CheckedOut => "Đã trả phòng",
            BookingStatus.Cancelled => "Đã hủy",
            _ => status.ToString(),
        };
    }
}
