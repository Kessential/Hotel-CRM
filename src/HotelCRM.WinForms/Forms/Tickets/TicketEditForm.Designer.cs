namespace HotelCRM.WinForms.Forms.Tickets;

partial class TicketEditForm
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
        lblCustomer = new Label();
        cboCustomer = new ComboBox();
        lblType = new Label();
        cboType = new ComboBox();
        lblPriority = new Label();
        cboPriority = new ComboBox();
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblRoom = new Label();
        cboRoom = new ComboBox();
        btnLoadRooms = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        SuspendLayout();
        //
        // lblCustomerKeyword
        //
        lblCustomerKeyword.AutoSize = true;
        lblCustomerKeyword.Location = new Point(12, 18);
        lblCustomerKeyword.Name = "lblCustomerKeyword";
        lblCustomerKeyword.Size = new Size(85, 15);
        lblCustomerKeyword.TabIndex = 0;
        lblCustomerKeyword.Text = "Find c&ustomer:";
        //
        // txtCustomerKeyword
        //
        txtCustomerKeyword.Location = new Point(110, 15);
        txtCustomerKeyword.MaxLength = 100;
        txtCustomerKeyword.Name = "txtCustomerKeyword";
        txtCustomerKeyword.PlaceholderText = "Name, phone or email";
        txtCustomerKeyword.Size = new Size(320, 23);
        txtCustomerKeyword.TabIndex = 1;
        //
        // btnFindCustomer
        //
        btnFindCustomer.Location = new Point(436, 13);
        btnFindCustomer.Name = "btnFindCustomer";
        btnFindCustomer.Size = new Size(110, 27);
        btnFindCustomer.TabIndex = 2;
        btnFindCustomer.Text = "&Find";
        btnFindCustomer.UseVisualStyleBackColor = true;
        btnFindCustomer.Click += btnFindCustomer_Click;
        //
        // lblCustomer
        //
        lblCustomer.AutoSize = true;
        lblCustomer.Location = new Point(12, 51);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Size = new Size(62, 15);
        lblCustomer.TabIndex = 3;
        lblCustomer.Text = "Customer:";
        //
        // cboCustomer
        //
        cboCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCustomer.Location = new Point(110, 48);
        cboCustomer.Name = "cboCustomer";
        cboCustomer.Size = new Size(436, 23);
        cboCustomer.TabIndex = 4;
        cboCustomer.SelectedIndexChanged += cboCustomer_SelectedIndexChanged;
        //
        // lblType
        //
        lblType.AutoSize = true;
        lblType.Location = new Point(12, 84);
        lblType.Name = "lblType";
        lblType.Size = new Size(34, 15);
        lblType.TabIndex = 5;
        lblType.Text = "&Type:";
        //
        // cboType
        //
        cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboType.Location = new Point(110, 81);
        cboType.Name = "cboType";
        cboType.Size = new Size(200, 23);
        cboType.TabIndex = 6;
        cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;
        //
        // lblPriority
        //
        lblPriority.AutoSize = true;
        lblPriority.Location = new Point(12, 117);
        lblPriority.Name = "lblPriority";
        lblPriority.Size = new Size(48, 15);
        lblPriority.TabIndex = 7;
        lblPriority.Text = "&Priority:";
        //
        // cboPriority
        //
        cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPriority.Location = new Point(110, 114);
        cboPriority.Name = "cboPriority";
        cboPriority.Size = new Size(200, 23);
        cboPriority.TabIndex = 8;
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(12, 150);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(32, 15);
        lblTitle.TabIndex = 9;
        lblTitle.Text = "T&itle:";
        //
        // txtTitle
        //
        txtTitle.Location = new Point(110, 147);
        txtTitle.MaxLength = 200;
        txtTitle.Name = "txtTitle";
        txtTitle.Size = new Size(436, 23);
        txtTitle.TabIndex = 10;
        //
        // lblDescription
        //
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(12, 183);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(70, 15);
        lblDescription.TabIndex = 11;
        lblDescription.Text = "&Description:";
        //
        // txtDescription
        //
        txtDescription.AcceptsReturn = true;
        txtDescription.Location = new Point(110, 180);
        txtDescription.MaxLength = 2000;
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(436, 150);
        txtDescription.TabIndex = 12;
        //
        // lblRoom
        //
        lblRoom.AutoSize = true;
        lblRoom.Location = new Point(12, 345);
        lblRoom.Name = "lblRoom";
        lblRoom.Size = new Size(42, 15);
        lblRoom.TabIndex = 13;
        lblRoom.Text = "&Room:";
        //
        // cboRoom
        //
        cboRoom.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRoom.Location = new Point(110, 342);
        cboRoom.Name = "cboRoom";
        cboRoom.Size = new Size(200, 23);
        cboRoom.TabIndex = 14;
        //
        // btnLoadRooms
        //
        btnLoadRooms.Location = new Point(316, 340);
        btnLoadRooms.Name = "btnLoadRooms";
        btnLoadRooms.Size = new Size(110, 27);
        btnLoadRooms.TabIndex = 15;
        btnLoadRooms.Text = "&Load Rooms";
        btnLoadRooms.UseVisualStyleBackColor = true;
        btnLoadRooms.Click += btnLoadRooms_Click;
        //
        // btnSave
        //
        btnSave.Location = new Point(370, 430);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(85, 27);
        btnSave.TabIndex = 16;
        btnSave.Text = "&Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(461, 430);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(85, 27);
        btnCancel.TabIndex = 17;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        //
        // TicketEditForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(560, 480);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(btnLoadRooms);
        Controls.Add(cboRoom);
        Controls.Add(lblRoom);
        Controls.Add(txtDescription);
        Controls.Add(lblDescription);
        Controls.Add(txtTitle);
        Controls.Add(lblTitle);
        Controls.Add(cboPriority);
        Controls.Add(lblPriority);
        Controls.Add(cboType);
        Controls.Add(lblType);
        Controls.Add(cboCustomer);
        Controls.Add(lblCustomer);
        Controls.Add(btnFindCustomer);
        Controls.Add(txtCustomerKeyword);
        Controls.Add(lblCustomerKeyword);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TicketEditForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Ticket";
        Load += TicketEditForm_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblCustomerKeyword;
    private TextBox txtCustomerKeyword;
    private Button btnFindCustomer;
    private Label lblCustomer;
    private ComboBox cboCustomer;
    private Label lblType;
    private ComboBox cboType;
    private Label lblPriority;
    private ComboBox cboPriority;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblRoom;
    private ComboBox cboRoom;
    private Button btnLoadRooms;
    private Button btnSave;
    private Button btnCancel;
}
