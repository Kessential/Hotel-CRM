using HotelCRM.Business.Security;
using HotelCRM.Business.Services;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.WinForms.Forms.Tickets;

/// <summary>
/// UC3 and UC5: search tickets and, for a manager, assign them. Opens the edit and detail forms.
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

    private void TicketListForm_Load(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            cboState.Items.Add(string.Empty);
            cboState.Items.AddRange([OpenState.StateName, InProgressState.StateName, ResolvedState.StateName, ClosedState.StateName]);
            cboState.SelectedIndex = 0;

            cboPriority.Items.Add(string.Empty);
            foreach (TicketPriority priority in Enum.GetValues<TicketPriority>())
            {
                cboPriority.Items.Add(priority);
            }

            cboPriority.SelectedIndex = 0;

            cboType.Items.Add(string.Empty);
            foreach (string typeName in _ticketService.GetTicketTypes())
            {
                cboType.Items.Add(typeName);
            }

            cboType.SelectedIndex = 0;

            IReadOnlyList<Agent> agents = _ticketService.GetAgents();
            cboAgent.Items.Add(string.Empty);
            foreach (Agent agent in agents)
            {
                cboAgent.Items.Add(new AgentItem(agent.Id, agent.FullName));
            }

            cboAgent.SelectedIndex = 0;

            bool canAssign = UserSession.Current.CanAssign();
            btnAutoAssign.Visible = canAssign;
            lblAssignAgent.Visible = canAssign;
            cboAssignAgent.Visible = canAssign;
            btnAssign.Visible = canAssign;
            if (canAssign)
            {
                foreach (Agent agent in agents)
                {
                    cboAssignAgent.Items.Add(new AgentItem(agent.Id, agent.FullName));
                }

                if (cboAssignAgent.Items.Count > 0)
                {
                    cboAssignAgent.SelectedIndex = 0;
                }
            }

            LoadTickets(null);
        });
    }

    private void btnSearch_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() => LoadTickets(null));
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            cboState.SelectedIndex = 0;
            cboPriority.SelectedIndex = 0;
            cboType.SelectedIndex = 0;
            cboAgent.SelectedIndex = 0;
            txtKeyword.Clear();
            LoadTickets(null);
        });
    }

    private void btnNew_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            using (TicketEditForm form = new(_ticketService))
            {
                form.ShowDialog(this);
            }

            LoadTickets(SelectedTicketId());
        });
    }

    private void btnDetails_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(OpenSelectedTicket);
    }

    private void dgvTickets_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            ErrorHandler.Run(OpenSelectedTicket);
        }
    }

    private void btnAutoAssign_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            int? ticketId = RequireSelectedTicket();
            if (ticketId is null)
            {
                return;
            }

            Agent agent = _ticketService.AutoAssign(UserSession.Current, ticketId.Value);
            LoadTickets(ticketId);
            MessageBox.Show(this, $"Ticket #{ticketId} was assigned to {agent.FullName}.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnAssign_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            int? ticketId = RequireSelectedTicket();
            if (ticketId is null)
            {
                return;
            }

            if (cboAssignAgent.SelectedItem is not AgentItem agent)
            {
                MessageBox.Show(this, "Select an agent first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _ticketService.AssignManually(UserSession.Current, ticketId.Value, agent.Id);
            LoadTickets(ticketId);
        });
    }

    private void btnExit_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(Close);
    }

    /// <summary>Opens the detail form for the selected ticket and reloads the grid when it closes.</summary>
    private void OpenSelectedTicket()
    {
        int? ticketId = RequireSelectedTicket();
        if (ticketId is null)
        {
            return;
        }

        using (TicketDetailForm form = new(_ticketService))
        {
            form.ShowTicket(ticketId.Value);
            form.ShowDialog(this);
        }

        LoadTickets(ticketId);
    }

    /// <summary>Runs the search with the current filters and selects <paramref name="selectTicketId"/> again if it is still listed.</summary>
    private void LoadTickets(int? selectTicketId)
    {
        string? stateName = cboState.SelectedItem as string;
        TicketPriority? priority = cboPriority.SelectedItem is TicketPriority p ? p : null;
        string? typeName = cboType.SelectedItem as string;
        int? agentId = (cboAgent.SelectedItem as AgentItem)?.Id;

        IReadOnlyList<SupportTicket> tickets = _ticketService.Search(stateName, priority, typeName, agentId, txtKeyword.Text);
        DateTime now = DateTime.Now;
        List<TicketRow> rows = tickets
            .Select(t => new TicketRow(
                t.Id,
                t.TypeName,
                t.Title,
                t.Priority.ToString(),
                t.State.Name,
                t.AssignedAgent?.FullName ?? "(unassigned)",
                t.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                t.DueAt.ToString("yyyy-MM-dd HH:mm"),
                t.IsOverdue(now) ? "Yes" : string.Empty))
            .ToList();

        dgvTickets.DataSource = rows;
        DataGridViewColumn? titleColumn = dgvTickets.Columns[nameof(TicketRow.Title)];
        if (titleColumn is not null)
        {
            titleColumn.FillWeight = 300;
        }
        lblCount.Text = $"{rows.Count} ticket(s)";

        if (selectTicketId is not null)
        {
            DataGridViewRow? match = dgvTickets.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(row => row.DataBoundItem is TicketRow ticketRow && ticketRow.Id == selectTicketId.Value);
            if (match is not null)
            {
                dgvTickets.CurrentCell = match.Cells[0];
            }
            else
            {
                // The ticket no longer matches the filters: select nothing, so the next action cannot
                // silently hit the first row instead.
                dgvTickets.ClearSelection();
                dgvTickets.CurrentCell = null;
            }
        }
    }

    /// <summary>Id of the highlighted row, or null when no row is highlighted.</summary>
    private int? SelectedTicketId()
    {
        return dgvTickets.CurrentRow is { Selected: true } row ? (row.DataBoundItem as TicketRow)?.Id : null;
    }

    /// <summary>Returns the selected ticket Id, or tells the user to select one and returns null.</summary>
    private int? RequireSelectedTicket()
    {
        int? ticketId = SelectedTicketId();
        if (ticketId is null)
        {
            MessageBox.Show(this, "Select a ticket first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        return ticketId;
    }

    /// <summary>One line of the grid; the property names are the column headers.</summary>
    private sealed record TicketRow(
        int Id,
        string Type,
        string Title,
        string Priority,
        string State,
        string Agent,
        string Created,
        string Due,
        string Overdue);

    /// <summary>An agent in a drop-down list.</summary>
    private sealed record AgentItem(int Id, string Name)
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
