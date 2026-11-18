namespace HotelCRM.WinForms.Forms.Bookings;

partial class BookingForm
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
        lblCustomerKeyword = new Label();
        txtCustomerKeyword = new TextBox();
        btnFindCustomer = new Button();
        cboCustomer = new ComboBox();
        lblCheckIn = new Label();
        dtpCheckIn = new DateTimePicker();
        lblCheckOut = new Label();
        dtpCheckOut = new DateTimePicker();
        btnFindRooms = new Button();
        cboRoom = new ComboBox();
        btnBook = new Button();
        dgvBookings = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colCustomer = new DataGridViewTextBoxColumn();
        colRoom = new DataGridViewTextBoxColumn();
        colRoomType = new DataGridViewTextBoxColumn();
        colCheckIn = new DataGridViewTextBoxColumn();
        colCheckOut = new DataGridViewTextBoxColumn();
        colNights = new DataGridViewTextBoxColumn();
        colTotal = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        btnCheckIn = new Button();
        btnCheckOut = new Button();
        btnCancelBooking = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvBookings).BeginInit();
        SuspendLayout();
        //
        // lblCustomerKeyword
        //
        lblCustomerKeyword.AutoSize = true;
        lblCustomerKeyword.Location = new Point(12, 17);
        lblCustomerKeyword.Name = "lblCustomerKeyword";
        lblCustomerKeyword.Size = new Size(62, 15);
        lblCustomerKeyword.TabIndex = 0;
        lblCustomerKeyword.Text = "C&ustomer:";
        //
        // txtCustomerKeyword
        //
        txtCustomerKeyword.Location = new Point(90, 14);
        txtCustomerKeyword.MaxLength = 100;
        txtCustomerKeyword.Name = "txtCustomerKeyword";
        txtCustomerKeyword.PlaceholderText = "Name, phone or email";
        txtCustomerKeyword.Size = new Size(180, 23);
        txtCustomerKeyword.TabIndex = 1;
        //
        // btnFindCustomer
        //
        btnFindCustomer.Location = new Point(276, 13);
        btnFindCustomer.Name = "btnFindCustomer";
        btnFindCustomer.Size = new Size(80, 25);
        btnFindCustomer.TabIndex = 2;
        btnFindCustomer.Text = "&Find";
        btnFindCustomer.UseVisualStyleBackColor = true;
        btnFindCustomer.Click += btnFindCustomer_Click;
        //
        // cboCustomer
        //
        cboCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCustomer.FormattingEnabled = true;
        cboCustomer.Location = new Point(362, 14);
        cboCustomer.Name = "cboCustomer";
        cboCustomer.Size = new Size(320, 23);
        cboCustomer.TabIndex = 3;
        //
        // lblCheckIn
        //
        lblCheckIn.AutoSize = true;
        lblCheckIn.Location = new Point(12, 53);
        lblCheckIn.Name = "lblCheckIn";
        lblCheckIn.Size = new Size(58, 15);
        lblCheckIn.TabIndex = 4;
        lblCheckIn.Text = "Check-&in:";
        //
        // dtpCheckIn
        //
        dtpCheckIn.Format = DateTimePickerFormat.Short;
        dtpCheckIn.Location = new Point(90, 49);
        dtpCheckIn.Name = "dtpCheckIn";
        dtpCheckIn.Size = new Size(120, 23);
        dtpCheckIn.TabIndex = 5;
        dtpCheckIn.ValueChanged += dtpStayDates_ValueChanged;
        //
        // lblCheckOut
        //
        lblCheckOut.AutoSize = true;
        lblCheckOut.Location = new Point(222, 53);
        lblCheckOut.Name = "lblCheckOut";
        lblCheckOut.Size = new Size(68, 15);
        lblCheckOut.TabIndex = 6;
        lblCheckOut.Text = "Check-ou&t:";
        //
        // dtpCheckOut
        //
        dtpCheckOut.Format = DateTimePickerFormat.Short;
        dtpCheckOut.Location = new Point(296, 49);
        dtpCheckOut.Name = "dtpCheckOut";
        dtpCheckOut.Size = new Size(120, 23);
        dtpCheckOut.TabIndex = 7;
        dtpCheckOut.ValueChanged += dtpStayDates_ValueChanged;
        //
        // btnFindRooms
        //
        btnFindRooms.Location = new Point(428, 48);
        btnFindRooms.Name = "btnFindRooms";
        btnFindRooms.Size = new Size(90, 25);
        btnFindRooms.TabIndex = 8;
        btnFindRooms.Text = "Find &rooms";
        btnFindRooms.UseVisualStyleBackColor = true;
        btnFindRooms.Click += btnFindRooms_Click;
        //
        // cboRoom
        //
        cboRoom.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRoom.FormattingEnabled = true;
        cboRoom.Location = new Point(524, 49);
        cboRoom.Name = "cboRoom";
        cboRoom.Size = new Size(300, 23);
        cboRoom.TabIndex = 9;
        //
        // btnBook
        //
        btnBook.Location = new Point(832, 48);
        btnBook.Name = "btnBook";
        btnBook.Size = new Size(80, 25);
        btnBook.TabIndex = 10;
        btnBook.Text = "&Book";
        btnBook.UseVisualStyleBackColor = true;
        btnBook.Click += btnBook_Click;
        //
        // dgvBookings
        //
        dgvBookings.AllowUserToAddRows = false;
        dgvBookings.AllowUserToDeleteRows = false;
        dgvBookings.AllowUserToResizeRows = false;
        dgvBookings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBookings.Columns.AddRange(new DataGridViewColumn[] { colId, colCustomer, colRoom, colRoomType, colCheckIn, colCheckOut, colNights, colTotal, colStatus });
        dgvBookings.Location = new Point(12, 88);
        dgvBookings.MultiSelect = false;
        dgvBookings.Name = "dgvBookings";
        dgvBookings.ReadOnly = true;
        dgvBookings.RowHeadersVisible = false;
        dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBookings.Size = new Size(936, 456);
        dgvBookings.TabIndex = 11;
        //
        // colId
        //
        colId.DataPropertyName = "Id";
        colId.FillWeight = 40F;
        colId.HeaderText = "Id";
        colId.Name = "colId";
        colId.ReadOnly = true;
        //
        // colCustomer
        //
        colCustomer.DataPropertyName = "Customer";
        colCustomer.FillWeight = 160F;
        colCustomer.HeaderText = "Customer";
        colCustomer.Name = "colCustomer";
        colCustomer.ReadOnly = true;
        //
        // colRoom
        //
        colRoom.DataPropertyName = "Room";
        colRoom.FillWeight = 60F;
        colRoom.HeaderText = "Room";
        colRoom.Name = "colRoom";
        colRoom.ReadOnly = true;
        //
        // colRoomType
        //
        colRoomType.DataPropertyName = "RoomType";
        colRoomType.FillWeight = 120F;
        colRoomType.HeaderText = "Room type";
        colRoomType.Name = "colRoomType";
        colRoomType.ReadOnly = true;
        //
        // colCheckIn
        //
        colCheckIn.DataPropertyName = "CheckIn";
        colCheckIn.HeaderText = "Check-in";
        colCheckIn.Name = "colCheckIn";
        colCheckIn.ReadOnly = true;
        //
        // colCheckOut
        //
        colCheckOut.DataPropertyName = "CheckOut";
        colCheckOut.HeaderText = "Check-out";
        colCheckOut.Name = "colCheckOut";
        colCheckOut.ReadOnly = true;
        //
        // colNights
        //
        colNights.DataPropertyName = "Nights";
        colNights.FillWeight = 50F;
        colNights.HeaderText = "Nights";
        colNights.Name = "colNights";
        colNights.ReadOnly = true;
        //
        // colTotal
        //
        colTotal.DataPropertyName = "Total";
        colTotal.FillWeight = 70F;
        colTotal.HeaderText = "Total (USD)";
        colTotal.Name = "colTotal";
        colTotal.ReadOnly = true;
        //
        // colStatus
        //
        colStatus.DataPropertyName = "Status";
        colStatus.FillWeight = 80F;
        colStatus.HeaderText = "Status";
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        //
        // btnCheckIn
        //
        btnCheckIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCheckIn.Location = new Point(12, 557);
        btnCheckIn.Name = "btnCheckIn";
        btnCheckIn.Size = new Size(90, 27);
        btnCheckIn.TabIndex = 12;
        btnCheckIn.Text = "Check In";
        btnCheckIn.UseVisualStyleBackColor = true;
        btnCheckIn.Click += btnCheckIn_Click;
        //
        // btnCheckOut
        //
        btnCheckOut.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCheckOut.Location = new Point(108, 557);
        btnCheckOut.Name = "btnCheckOut";
        btnCheckOut.Size = new Size(90, 27);
        btnCheckOut.TabIndex = 13;
        btnCheckOut.Text = "Check Out";
        btnCheckOut.UseVisualStyleBackColor = true;
        btnCheckOut.Click += btnCheckOut_Click;
        //
        // btnCancelBooking
        //
        btnCancelBooking.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCancelBooking.Location = new Point(204, 557);
        btnCancelBooking.Name = "btnCancelBooking";
        btnCancelBooking.Size = new Size(110, 27);
        btnCancelBooking.TabIndex = 14;
        btnCancelBooking.Text = "Cancel Booking";
        btnCancelBooking.UseVisualStyleBackColor = true;
        btnCancelBooking.Click += btnCancelBooking_Click;
        //
        // btnClose
        //
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(868, 557);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(80, 27);
        btnClose.TabIndex = 15;
        btnClose.Text = "&Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        //
        // BookingForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(960, 600);
        Controls.Add(btnClose);
        Controls.Add(btnCancelBooking);
        Controls.Add(btnCheckOut);
        Controls.Add(btnCheckIn);
        Controls.Add(dgvBookings);
        Controls.Add(btnBook);
        Controls.Add(cboRoom);
        Controls.Add(btnFindRooms);
        Controls.Add(dtpCheckOut);
        Controls.Add(lblCheckOut);
        Controls.Add(dtpCheckIn);
        Controls.Add(lblCheckIn);
        Controls.Add(cboCustomer);
        Controls.Add(btnFindCustomer);
        Controls.Add(txtCustomerKeyword);
        Controls.Add(lblCustomerKeyword);
        MinimizeBox = false;
        MinimumSize = new Size(940, 400);
        Name = "BookingForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Bookings";
        Load += BookingForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvBookings).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblCustomerKeyword;
    private TextBox txtCustomerKeyword;
    private Button btnFindCustomer;
    private ComboBox cboCustomer;
    private Label lblCheckIn;
    private DateTimePicker dtpCheckIn;
    private Label lblCheckOut;
    private DateTimePicker dtpCheckOut;
    private Button btnFindRooms;
    private ComboBox cboRoom;
    private Button btnBook;
    private DataGridView dgvBookings;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colCustomer;
    private DataGridViewTextBoxColumn colRoom;
    private DataGridViewTextBoxColumn colRoomType;
    private DataGridViewTextBoxColumn colCheckIn;
    private DataGridViewTextBoxColumn colCheckOut;
    private DataGridViewTextBoxColumn colNights;
    private DataGridViewTextBoxColumn colTotal;
    private DataGridViewTextBoxColumn colStatus;
    private Button btnCheckIn;
    private Button btnCheckOut;
    private Button btnCancelBooking;
    private Button btnClose;
}
