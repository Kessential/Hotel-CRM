using HotelCRM.Business.Security;
using HotelCRM.Business.Services;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC4: show one ticket and start, resolve, close or reopen it. Opened empty from the menu (enter an Id
/// and click Load) or with a ticket already shown by <see cref="ShowTicket"/>.
/// </summary>
public partial class TicketDetailForm : Form
{
    private const string DateFormat = "yyyy-MM-dd HH:mm";

    private readonly TicketService _ticketService;
    private int? _ticketId;
    private int? _roomId;

    /// <summary>Creates the form.</summary>
    /// <param name="ticketService">Loads the ticket and performs the state changes.</param>
    public TicketDetailForm(TicketService ticketService)
    {
        _ticketService = ticketService;
        InitializeComponent();
    }

    /// <summary>Loads and shows a ticket; called by the ticket list before <see cref="Form.ShowDialog()"/>.</summary>
    /// <param name="ticketId">Id of the ticket.</param>
    /// <exception cref="BusinessRuleException">The ticket does not exist.</exception>
    public void ShowTicket(int ticketId)
    {
        numTicketId.Value = Math.Clamp(ticketId, (int)numTicketId.Minimum, (int)numTicketId.Maximum);
        LoadTicket(ticketId);
    }

    private void TicketDetailForm_Load(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            // Hiding a button is only a convenience: the domain layer checks the permission again.
            Employee user = UserSession.Current;
            bool isAgent = user.Role == EmployeeRole.Agent;
            lblNote.Visible = isAgent;
            txtNote.Visible = isAgent;
            btnStart.Visible = isAgent;
            btnResolve.Visible = isAgent;
            btnClose.Visible = user.CanClose();
            btnReopen.Visible = user.CanReopen();
            UpdateActionButtons();
        });
    }

    private void btnLoad_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            // Clear first: if the Id does not exist, the form stays empty instead of keeping the previous ticket.
            ClearTicket();
            LoadTicket((int)numTicketId.Value);
        });
    }

    private void btnRoomNumber_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (_roomId is not null)
            {
                txtRoom.Text = $"{_ticketService.GetRoomNumber(_roomId.Value)} (Id {_roomId})";
            }
        });
    }

    private void btnStart_Click(object? sender, EventArgs e)
    {
        RunOnTicket(id => _ticketService.Start(UserSession.Current, id, DateTime.Now));
    }

    private void btnResolve_Click(object? sender, EventArgs e)
    {
        bool resolved = RunOnTicket(id => _ticketService.Resolve(UserSession.Current, id, txtNote.Text.Trim(), DateTime.Now));
        if (resolved)
        {
            txtNote.Clear();
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        RunOnTicket(id => _ticketService.Close(UserSession.Current, id, DateTime.Now));
    }

    private void btnReopen_Click(object? sender, EventArgs e)
    {
        RunOnTicket(id => _ticketService.Reopen(UserSession.Current, id, DateTime.Now));
    }

    private void btnExit_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(Close);
    }

    /// <summary>
    /// Runs a state change on the shown ticket, then reloads it from the service. The reload also runs
    /// when the change failed, because an observer may fail after the state has already changed.
    /// </summary>
    /// <returns>True when the change succeeded.</returns>
    private bool RunOnTicket(Action<int> action)
    {
        if (_ticketId is not int ticketId)
        {
            ErrorHandler.Run(() =>
                MessageBox.Show(this, "Load a ticket first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information));
            return false;
        }

        bool succeeded = ErrorHandler.Run(() => action(ticketId));
        ErrorHandler.Run(() => LoadTicket(ticketId));
        return succeeded;
    }

    /// <summary>Empties the form and disables the action buttons until a ticket is loaded.</summary>
    private void ClearTicket()
    {
        _ticketId = null;
        _roomId = null;

        Text = "Ticket Details";
        txtType.Clear();
        txtState.Clear();
        txtPriority.Clear();
        txtAgent.Clear();
        txtCreated.Clear();
        txtDue.Clear();
        txtResolved.Clear();
        txtClosed.Clear();
        txtTitle.Clear();
        txtDescription.Clear();
        txtResolutionNote.Clear();
        txtRoom.Clear();
        lblRoom.Visible = false;
        txtRoom.Visible = false;
        btnRoomNumber.Visible = false;

        UpdateActionButtons();
    }

    private void LoadTicket(int ticketId)
    {
        SupportTicket ticket = _ticketService.GetById(ticketId);
        _ticketId = ticket.Id;
        _roomId = ticket is MaintenanceTicket maintenance ? maintenance.RoomId : null;

        Text = $"Ticket #{ticket.Id}";
        txtType.Text = ticket.TypeName;
        txtState.Text = ticket.State.Name;
        txtPriority.Text = ticket.Priority.ToString();
        txtAgent.Text = ticket.AssignedAgent?.FullName ?? "(unassigned)";
        txtCreated.Text = ticket.CreatedAt.ToString(DateFormat);
        txtDue.Text = ticket.IsOverdue(DateTime.Now)
            ? $"{ticket.DueAt.ToString(DateFormat)} (overdue)"
            : ticket.DueAt.ToString(DateFormat);
        txtResolved.Text = ticket.ResolvedAt?.ToString(DateFormat) ?? string.Empty;
        txtClosed.Text = ticket.ClosedAt?.ToString(DateFormat) ?? string.Empty;
        txtTitle.Text = ticket.Title;
        txtDescription.Text = ticket.Description;
        txtResolutionNote.Text = ticket.ResolutionNote ?? string.Empty;

        // The room number lives in the other subsystem: it is looked up only when the user asks for it.
        bool hasRoom = _roomId is not null;
        lblRoom.Visible = hasRoom;
        txtRoom.Visible = hasRoom;
        btnRoomNumber.Visible = hasRoom;
        txtRoom.Text = hasRoom ? $"Id {_roomId}" : string.Empty;

        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        bool loaded = _ticketId is not null;
        btnStart.Enabled = loaded;
        btnResolve.Enabled = loaded;
        btnClose.Enabled = loaded;
        btnReopen.Enabled = loaded;
    }
}
