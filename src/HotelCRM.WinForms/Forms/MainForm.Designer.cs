namespace HotelCRM.WinForms.Forms;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        mnuMain = new MenuStrip();
        mnuTickets = new ToolStripMenuItem();
        mnuTicketList = new ToolStripMenuItem();
        mnuNewTicket = new ToolStripMenuItem();
        mnuTicketDetails = new ToolStripMenuItem();
        mnuCustomers = new ToolStripMenuItem();
        mnuCustomerList = new ToolStripMenuItem();
        mnuNewCustomer = new ToolStripMenuItem();
        mnuCustomerHistory = new ToolStripMenuItem();
        mnuRecordSurvey = new ToolStripMenuItem();
        mnuBookings = new ToolStripMenuItem();
        mnuManageBookings = new ToolStripMenuItem();
        mnuReports = new ToolStripMenuItem();
        mnuViewReports = new ToolStripMenuItem();
        mnuAccount = new ToolStripMenuItem();
        mnuLogOut = new ToolStripMenuItem();
        mnuExit = new ToolStripMenuItem();
        stsMain = new StatusStrip();
        lblNotification = new ToolStripStatusLabel();
        lblWelcome = new Label();
        lblRole = new Label();
        lblHint = new Label();
        mnuMain.SuspendLayout();
        stsMain.SuspendLayout();
        SuspendLayout();
        //
        // mnuMain
        //
        mnuMain.Items.AddRange(new ToolStripItem[] { mnuTickets, mnuCustomers, mnuBookings, mnuReports, mnuAccount });
        mnuMain.Location = new Point(0, 0);
        mnuMain.Name = "mnuMain";
        mnuMain.Size = new Size(784, 24);
        mnuMain.TabIndex = 0;
        mnuMain.Text = "Main menu";
        //
        // mnuTickets
        //
        mnuTickets.DropDownItems.AddRange(new ToolStripItem[] { mnuTicketList, mnuNewTicket, mnuTicketDetails });
        mnuTickets.Name = "mnuTickets";
        mnuTickets.Size = new Size(56, 20);
        mnuTickets.Text = "&Tickets";
        //
        // mnuTicketList
        //
        mnuTicketList.Name = "mnuTicketList";
        mnuTicketList.Size = new Size(180, 22);
        mnuTicketList.Text = "Ticket &List";
        mnuTicketList.Click += mnuTicketList_Click;
        //
        // mnuNewTicket
        //
        mnuNewTicket.Name = "mnuNewTicket";
        mnuNewTicket.Size = new Size(180, 22);
        mnuNewTicket.Text = "&New Ticket";
        mnuNewTicket.Click += mnuNewTicket_Click;
        //
        // mnuTicketDetails
        //
        mnuTicketDetails.Name = "mnuTicketDetails";
        mnuTicketDetails.Size = new Size(180, 22);
        mnuTicketDetails.Text = "Ticket &Details";
        mnuTicketDetails.Click += mnuTicketDetails_Click;
        //
        // mnuCustomers
        //
        mnuCustomers.DropDownItems.AddRange(new ToolStripItem[] { mnuCustomerList, mnuNewCustomer, mnuCustomerHistory, mnuRecordSurvey });
        mnuCustomers.Name = "mnuCustomers";
        mnuCustomers.Size = new Size(76, 20);
        mnuCustomers.Text = "&Customers";
        //
        // mnuCustomerList
        //
        mnuCustomerList.Name = "mnuCustomerList";
        mnuCustomerList.Size = new Size(180, 22);
        mnuCustomerList.Text = "Customer &List";
        mnuCustomerList.Click += mnuCustomerList_Click;
        //
        // mnuNewCustomer
        //
        mnuNewCustomer.Name = "mnuNewCustomer";
        mnuNewCustomer.Size = new Size(180, 22);
        mnuNewCustomer.Text = "&New Customer";
        mnuNewCustomer.Click += mnuNewCustomer_Click;
        //
        // mnuCustomerHistory
        //
        mnuCustomerHistory.Name = "mnuCustomerHistory";
        mnuCustomerHistory.Size = new Size(180, 22);
        mnuCustomerHistory.Text = "Interaction &History";
        mnuCustomerHistory.Click += mnuCustomerHistory_Click;
        //
        // mnuRecordSurvey
        //
        mnuRecordSurvey.Name = "mnuRecordSurvey";
        mnuRecordSurvey.Size = new Size(180, 22);
        mnuRecordSurvey.Text = "Record &Survey";
        mnuRecordSurvey.Click += mnuRecordSurvey_Click;
        //
        // mnuBookings
        //
        mnuBookings.DropDownItems.AddRange(new ToolStripItem[] { mnuManageBookings });
        mnuBookings.Name = "mnuBookings";
        mnuBookings.Size = new Size(68, 20);
        mnuBookings.Text = "&Bookings";
        //
        // mnuManageBookings
        //
        mnuManageBookings.Name = "mnuManageBookings";
        mnuManageBookings.Size = new Size(180, 22);
        mnuManageBookings.Text = "&Manage Bookings";
        mnuManageBookings.Click += mnuManageBookings_Click;
        //
        // mnuReports
        //
        mnuReports.DropDownItems.AddRange(new ToolStripItem[] { mnuViewReports });
        mnuReports.Name = "mnuReports";
        mnuReports.Size = new Size(59, 20);
        mnuReports.Text = "&Reports";
        //
        // mnuViewReports
        //
        mnuViewReports.Name = "mnuViewReports";
        mnuViewReports.Size = new Size(180, 22);
        mnuViewReports.Text = "&View Reports";
        mnuViewReports.Click += mnuViewReports_Click;
        //
        // mnuAccount
        //
        mnuAccount.DropDownItems.AddRange(new ToolStripItem[] { mnuLogOut, mnuExit });
        mnuAccount.Name = "mnuAccount";
        mnuAccount.Size = new Size(64, 20);
        mnuAccount.Text = "&Account";
        //
        // mnuLogOut
        //
        mnuLogOut.Name = "mnuLogOut";
        mnuLogOut.Size = new Size(180, 22);
        mnuLogOut.Text = "&Log Out";
        mnuLogOut.Click += mnuLogOut_Click;
        //
        // mnuExit
        //
        mnuExit.Name = "mnuExit";
        mnuExit.Size = new Size(180, 22);
        mnuExit.Text = "E&xit";
        mnuExit.Click += mnuExit_Click;
        //
        // stsMain
        //
        stsMain.Items.AddRange(new ToolStripItem[] { lblNotification });
        stsMain.Location = new Point(0, 439);
        stsMain.Name = "stsMain";
        stsMain.Size = new Size(784, 22);
        stsMain.TabIndex = 1;
        stsMain.Text = "Status bar";
        //
        // lblNotification
        //
        lblNotification.Name = "lblNotification";
        lblNotification.Size = new Size(39, 17);
        lblNotification.Text = "Ready";
        //
        // lblWelcome
        //
        lblWelcome.AutoSize = true;
        lblWelcome.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblWelcome.Location = new Point(24, 48);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Size = new Size(93, 25);
        lblWelcome.TabIndex = 2;
        lblWelcome.Text = "Welcome";
        //
        // lblRole
        //
        lblRole.AutoSize = true;
        lblRole.Location = new Point(26, 82);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(30, 15);
        lblRole.TabIndex = 3;
        lblRole.Text = "Role";
        //
        // lblHint
        //
        lblHint.AutoSize = true;
        lblHint.Location = new Point(26, 116);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(215, 15);
        lblHint.TabIndex = 4;
        lblHint.Text = "Use the menu above to open a function.";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 461);
        Controls.Add(lblHint);
        Controls.Add(lblRole);
        Controls.Add(lblWelcome);
        Controls.Add(stsMain);
        Controls.Add(mnuMain);
        MainMenuStrip = mnuMain;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Hotel CRM";
        mnuMain.ResumeLayout(false);
        mnuMain.PerformLayout();
        stsMain.ResumeLayout(false);
        stsMain.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip mnuMain;
    private ToolStripMenuItem mnuTickets;
    private ToolStripMenuItem mnuTicketList;
    private ToolStripMenuItem mnuNewTicket;
    private ToolStripMenuItem mnuTicketDetails;
    private ToolStripMenuItem mnuCustomers;
    private ToolStripMenuItem mnuCustomerList;
    private ToolStripMenuItem mnuNewCustomer;
    private ToolStripMenuItem mnuCustomerHistory;
    private ToolStripMenuItem mnuRecordSurvey;
    private ToolStripMenuItem mnuBookings;
    private ToolStripMenuItem mnuManageBookings;
    private ToolStripMenuItem mnuReports;
    private ToolStripMenuItem mnuViewReports;
    private ToolStripMenuItem mnuAccount;
    private ToolStripMenuItem mnuLogOut;
    private ToolStripMenuItem mnuExit;
    private StatusStrip stsMain;
    private ToolStripStatusLabel lblNotification;
    private Label lblWelcome;
    private Label lblRole;
    private Label lblHint;
}
