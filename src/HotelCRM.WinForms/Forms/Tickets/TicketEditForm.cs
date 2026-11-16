using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC2: create a ticket for a customer. Content is added at stage 2.
/// </summary>
public partial class TicketEditForm : Form
{
    private readonly TicketService _ticketService;

    /// <summary>Creates the form.</summary>
    /// <param name="ticketService">Creates the ticket and supplies the customer, type and room lists.</param>
    public TicketEditForm(TicketService ticketService)
    {
        _ticketService = ticketService;
        InitializeComponent();
    }
}
