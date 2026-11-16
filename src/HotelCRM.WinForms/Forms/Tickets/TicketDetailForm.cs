using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC4: show one ticket and start, resolve, close or reopen it. Content is added at stage 2.
/// </summary>
public partial class TicketDetailForm : Form
{
    private readonly TicketService _ticketService;

    /// <summary>Creates the form.</summary>
    /// <param name="ticketService">Loads the ticket and performs the state changes.</param>
    public TicketDetailForm(TicketService ticketService)
    {
        _ticketService = ticketService;
        InitializeComponent();
    }
}
