using HotelCRM.Business.Services;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC2: create a ticket for a customer. The customer list and the room list come from the customer
/// subsystem, so they are loaded only when the user clicks Find or Load Rooms.
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

    private void TicketEditForm_Load(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            foreach (string typeName in _ticketService.GetTicketTypes())
            {
                cboType.Items.Add(typeName);
            }

            if (cboType.Items.Count > 0)
            {
                cboType.SelectedIndex = 0;
            }

            foreach (TicketPriority priority in Enum.GetValues<TicketPriority>())
            {
                cboPriority.Items.Add(priority);
            }

            cboPriority.SelectedItem = TicketPriority.Medium;
            UpdateRoomControls();
        });
    }

    private void btnFindCustomer_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            cboCustomer.Items.Clear();
            ClearRooms();
            foreach (CustomerSummary customer in _ticketService.FindCustomers(txtCustomerKeyword.Text.Trim()))
            {
                cboCustomer.Items.Add(new CustomerItem(customer.Id, $"{customer.FullName} - {customer.Phone} ({customer.TierName})"));
            }

            if (cboCustomer.Items.Count > 0)
            {
                cboCustomer.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(this, "No customer matches the keyword.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void cboCustomer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ErrorHandler.Run(ClearRooms);
    }

    private void cboType_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ErrorHandler.Run(UpdateRoomControls);
    }

    private void btnLoadRooms_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            ClearRooms();
            if (cboCustomer.SelectedItem is not CustomerItem customer)
            {
                MessageBox.Show(this, "Select a customer first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (KeyValuePair<int, string> room in _ticketService.GetActiveRooms(customer.Id))
            {
                cboRoom.Items.Add(new RoomItem(room.Key, room.Value));
            }

            if (cboRoom.Items.Count > 0)
            {
                cboRoom.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(this, "This customer is not checked in to any room.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (cboCustomer.SelectedItem is not CustomerItem customer)
            {
                MessageBox.Show(this, "Find and select a customer first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string typeName = (string)cboType.SelectedItem!;
            TicketPriority priority = (TicketPriority)cboPriority.SelectedItem!;
            int? roomId = cboRoom.Enabled ? (cboRoom.SelectedItem as RoomItem)?.Id : null;

            SupportTicket ticket = _ticketService.CreateTicket(
                typeName, customer.Id, txtTitle.Text.Trim(), txtDescription.Text.Trim(), priority, roomId, DateTime.Now);

            MessageBox.Show(
                this,
                $"Ticket #{ticket.Id} was created.\nPriority: {ticket.Priority}\nDue: {ticket.DueAt:yyyy-MM-dd HH:mm}",
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        });
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        });
    }

    /// <summary>The room list is used only by maintenance tickets.</summary>
    private void UpdateRoomControls()
    {
        bool needsRoom = cboType.SelectedItem as string == MaintenanceTicket.TypeKey;
        cboRoom.Enabled = needsRoom;
        btnLoadRooms.Enabled = needsRoom;
        if (!needsRoom)
        {
            ClearRooms();
        }
    }

    private void ClearRooms()
    {
        cboRoom.Items.Clear();
    }

    /// <summary>A customer in the drop-down list.</summary>
    private sealed record CustomerItem(int Id, string Text)
    {
        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>A room in the drop-down list; shows the room number.</summary>
    private sealed record RoomItem(int Id, string Number)
    {
        public override string ToString()
        {
            return Number;
        }
    }
}
