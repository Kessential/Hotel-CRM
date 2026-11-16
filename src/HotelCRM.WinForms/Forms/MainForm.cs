using HotelCRM.Business.Security;
using HotelCRM.Business.Services;
using HotelCRM.Domain.Common;
using HotelCRM.WinForms.Forms.Bookings;
using HotelCRM.WinForms.Forms.Customers;
using HotelCRM.WinForms.Forms.Reports;
using HotelCRM.WinForms.Forms.Tickets;

namespace HotelCRM.WinForms.Forms;

/// <summary>
/// Main window: a menu that opens the other forms as dialogs and a status bar that shows ticket
/// notifications. The menu depends on the role of the logged-in employee (rule 11): only a manager
/// sees Reports, only an agent sees Record Survey. Hiding a menu item is a convenience; the real
/// permission checks are in the domain layer.
/// </summary>
public partial class MainForm : Form
{
    private readonly AuthService _authService;
    private readonly TicketService _ticketService;
    private readonly CustomerService _customerService;
    private readonly BookingService _bookingService;
    private readonly SurveyService _surveyService;
    private readonly ReportService _reportService;
    private readonly UiNotificationObserver _notifications;

    /// <summary>Creates the main window for the employee who has just logged in.</summary>
    /// <param name="authService">Used to log out.</param>
    /// <param name="ticketService">Passed to the ticket forms.</param>
    /// <param name="customerService">Passed to the customer, survey and booking forms.</param>
    /// <param name="bookingService">Passed to the booking form.</param>
    /// <param name="surveyService">Passed to the survey form.</param>
    /// <param name="reportService">Passed to the report form.</param>
    /// <param name="notifications">Observer whose notifications are shown in the status bar.</param>
    public MainForm(
        AuthService authService,
        TicketService ticketService,
        CustomerService customerService,
        BookingService bookingService,
        SurveyService surveyService,
        ReportService reportService,
        UiNotificationObserver notifications)
    {
        _authService = authService;
        _ticketService = ticketService;
        _customerService = customerService;
        _bookingService = bookingService;
        _surveyService = surveyService;
        _reportService = reportService;
        _notifications = notifications;

        InitializeComponent();
        ShowCurrentUser();
        _notifications.NotificationRaised += ShowNotification;
    }

    /// <summary>
    /// True when the window was closed with Log Out, so <c>Program</c> shows the login form again;
    /// false when the user chose Exit or closed the window.
    /// </summary>
    public bool LoggedOut { get; private set; }

    /// <summary>Stops listening to notifications when the window closes; the observer lives longer than this form.</summary>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _notifications.NotificationRaised -= ShowNotification;
        base.OnFormClosed(e);
    }

    /// <summary>Shows who is logged in and hides the menu items the role may not use.</summary>
    private void ShowCurrentUser()
    {
        Employee user = UserSession.Current;
        bool isManager = user.Role == EmployeeRole.Manager;

        lblWelcome.Text = $"Welcome, {user.FullName}";
        lblRole.Text = $"{user.Role} - {user.Department.Name}";
        mnuReports.Visible = isManager;
        mnuRecordSurvey.Visible = !isManager;
    }

    private void ShowNotification(string message)
    {
        lblNotification.Text = message;
    }

    /// <summary>Shows a form as a dialog on top of this window and disposes it when it closes.</summary>
    private void ShowDialogForm(Form form)
    {
        using (form)
        {
            form.ShowDialog(this);
        }
    }

    private void mnuTicketList_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new TicketListForm(_ticketService)));
    }

    private void mnuNewTicket_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new TicketEditForm(_ticketService)));
    }

    private void mnuTicketDetails_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new TicketDetailForm(_ticketService)));
    }

    private void mnuCustomerList_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new CustomerListForm(_customerService)));
    }

    private void mnuNewCustomer_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new CustomerEditForm(_customerService)));
    }

    private void mnuCustomerHistory_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new CustomerHistoryForm(_customerService)));
    }

    private void mnuRecordSurvey_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new SurveyForm(_surveyService, _customerService)));
    }

    private void mnuManageBookings_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new BookingForm(_bookingService, _customerService)));
    }

    private void mnuViewReports_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => ShowDialogForm(new ReportForm(_reportService)));
    }

    private void mnuLogOut_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            _authService.Logout();
            LoggedOut = true;
            Close();
        });
    }

    private void mnuExit_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(Close);
    }
}
