using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC3 and UC5: search tickets and, for a manager, assign them. Opens the edit and detail forms. Content is added at stage 2.
/// </summary>
public partial class TicketListForm : Form
{
    private readonly TicketService _ticketService;

    /// <summary>Creates the form.</summary>
    /// <param name="ticketService">Searches and assigns tickets.</param>
    public TicketListForm(TicketService ticketService)
    {
        _ticketService = ticketService;
        InitializeComponent();
    }
}
