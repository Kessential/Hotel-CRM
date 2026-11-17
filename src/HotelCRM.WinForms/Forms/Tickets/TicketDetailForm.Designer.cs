namespace HotelCRM.WinForms.Forms.Tickets;

partial class TicketDetailForm
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
        lblTicketId = new Label();
        numTicketId = new NumericUpDown();
        btnLoad = new Button();
        lblType = new Label();
        txtType = new TextBox();
        lblState = new Label();
        txtState = new TextBox();
        lblPriority = new Label();
        txtPriority = new TextBox();
        lblAgent = new Label();
        txtAgent = new TextBox();
        lblCreated = new Label();
        txtCreated = new TextBox();
        lblDue = new Label();
        txtDue = new TextBox();
        lblResolved = new Label();
        txtResolved = new TextBox();
        lblClosed = new Label();
        txtClosed = new TextBox();
        lblRoom = new Label();
        txtRoom = new TextBox();
        btnRoomNumber = new Button();
        lblTitle = new Label();
        txtTitle = new TextBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblResolutionNote = new Label();
        txtResolutionNote = new TextBox();
        lblNote = new Label();
        txtNote = new TextBox();
        btnStart = new Button();
        btnResolve = new Button();
        btnClose = new Button();
        btnReopen = new Button();
        btnExit = new Button();
        ((System.ComponentModel.ISupportInitialize)numTicketId).BeginInit();
        SuspendLayout();
        //
        // lblTicketId
        //
        lblTicketId.AutoSize = true;
        lblTicketId.Location = new Point(12, 18);
        lblTicketId.Name = "lblTicketId";
        lblTicketId.Size = new Size(54, 15);
        lblTicketId.TabIndex = 0;
        lblTicketId.Text = "Ticket &Id:";
        //
        // numTicketId
        //
        numTicketId.Location = new Point(100, 15);
        numTicketId.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numTicketId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numTicketId.Name = "numTicketId";
        numTicketId.Size = new Size(100, 23);
        numTicketId.TabIndex = 1;
        numTicketId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // btnLoad
        //
        btnLoad.Location = new Point(206, 13);
        btnLoad.Name = "btnLoad";
        btnLoad.Size = new Size(85, 27);
        btnLoad.TabIndex = 2;
        btnLoad.Text = "&Load";
        btnLoad.UseVisualStyleBackColor = true;
        btnLoad.Click += btnLoad_Click;
        //
        // lblType
        //
        lblType.AutoSize = true;
        lblType.Location = new Point(12, 58);
        lblType.Name = "lblType";
        lblType.Size = new Size(60, 15);
        lblType.TabIndex = 3;
        lblType.Text = "Type:";
        //
        // txtType
        //
        txtType.Location = new Point(100, 55);
        txtType.Name = "txtType";
        txtType.ReadOnly = true;
        txtType.Size = new Size(200, 23);
        txtType.TabIndex = 4;
        //
        // lblState
        //
        lblState.AutoSize = true;
        lblState.Location = new Point(330, 58);
        lblState.Name = "lblState";
        lblState.Size = new Size(60, 15);
        lblState.TabIndex = 5;
        lblState.Text = "State:";
        //
        // txtState
        //
        txtState.Location = new Point(420, 55);
        txtState.Name = "txtState";
        txtState.ReadOnly = true;
        txtState.Size = new Size(200, 23);
        txtState.TabIndex = 6;
        //
        // lblPriority
        //
        lblPriority.AutoSize = true;
        lblPriority.Location = new Point(12, 88);
        lblPriority.Name = "lblPriority";
        lblPriority.Size = new Size(60, 15);
        lblPriority.TabIndex = 7;
        lblPriority.Text = "Priority:";
        //
        // txtPriority
        //
        txtPriority.Location = new Point(100, 85);
        txtPriority.Name = "txtPriority";
        txtPriority.ReadOnly = true;
        txtPriority.Size = new Size(200, 23);
        txtPriority.TabIndex = 8;
        //
        // lblAgent
        //
        lblAgent.AutoSize = true;
        lblAgent.Location = new Point(330, 88);
        lblAgent.Name = "lblAgent";
        lblAgent.Size = new Size(60, 15);
        lblAgent.TabIndex = 9;
        lblAgent.Text = "Agent:";
        //
        // txtAgent
        //
        txtAgent.Location = new Point(420, 85);
        txtAgent.Name = "txtAgent";
        txtAgent.ReadOnly = true;
        txtAgent.Size = new Size(200, 23);
        txtAgent.TabIndex = 10;
        //
        // lblCreated
        //
        lblCreated.AutoSize = true;
        lblCreated.Location = new Point(12, 118);
        lblCreated.Name = "lblCreated";
        lblCreated.Size = new Size(60, 15);
        lblCreated.TabIndex = 11;
        lblCreated.Text = "Created:";
        //
        // txtCreated
        //
        txtCreated.Location = new Point(100, 115);
        txtCreated.Name = "txtCreated";
        txtCreated.ReadOnly = true;
        txtCreated.Size = new Size(200, 23);
        txtCreated.TabIndex = 12;
        //
        // lblDue
        //
        lblDue.AutoSize = true;
        lblDue.Location = new Point(330, 118);
        lblDue.Name = "lblDue";
        lblDue.Size = new Size(60, 15);
        lblDue.TabIndex = 13;
        lblDue.Text = "Due:";
        //
        // txtDue
        //
        txtDue.Location = new Point(420, 115);
        txtDue.Name = "txtDue";
        txtDue.ReadOnly = true;
        txtDue.Size = new Size(200, 23);
        txtDue.TabIndex = 14;
        //
        // lblResolved
        //
        lblResolved.AutoSize = true;
        lblResolved.Location = new Point(12, 148);
        lblResolved.Name = "lblResolved";
        lblResolved.Size = new Size(60, 15);
        lblResolved.TabIndex = 15;
        lblResolved.Text = "Resolved:";
        //
        // txtResolved
        //
        txtResolved.Location = new Point(100, 145);
        txtResolved.Name = "txtResolved";
        txtResolved.ReadOnly = true;
        txtResolved.Size = new Size(200, 23);
        txtResolved.TabIndex = 16;
        //
        // lblClosed
        //
        lblClosed.AutoSize = true;
        lblClosed.Location = new Point(330, 148);
        lblClosed.Name = "lblClosed";
        lblClosed.Size = new Size(60, 15);
        lblClosed.TabIndex = 17;
        lblClosed.Text = "Closed:";
        //
        // txtClosed
        //
        txtClosed.Location = new Point(420, 145);
        txtClosed.Name = "txtClosed";
        txtClosed.ReadOnly = true;
        txtClosed.Size = new Size(200, 23);
        txtClosed.TabIndex = 18;
        //
        // lblRoom
        //
        lblRoom.AutoSize = true;
        lblRoom.Location = new Point(12, 178);
        lblRoom.Name = "lblRoom";
        lblRoom.Size = new Size(42, 15);
        lblRoom.TabIndex = 19;
        lblRoom.Text = "Room:";
        //
        // txtRoom
        //
        txtRoom.Location = new Point(100, 175);
        txtRoom.Name = "txtRoom";
        txtRoom.ReadOnly = true;
        txtRoom.Size = new Size(200, 23);
        txtRoom.TabIndex = 20;
        //
        // btnRoomNumber
        //
        btnRoomNumber.Location = new Point(306, 173);
        btnRoomNumber.Name = "btnRoomNumber";
        btnRoomNumber.Size = new Size(110, 27);
        btnRoomNumber.TabIndex = 21;
        btnRoomNumber.Text = "Room &Number";
        btnRoomNumber.UseVisualStyleBackColor = true;
        btnRoomNumber.Click += btnRoomNumber_Click;
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(12, 208);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(32, 15);
        lblTitle.TabIndex = 22;
        lblTitle.Text = "Title:";
        //
        // txtTitle
        //
        txtTitle.Location = new Point(100, 205);
        txtTitle.Name = "txtTitle";
        txtTitle.ReadOnly = true;
        txtTitle.Size = new Size(520, 23);
        txtTitle.TabIndex = 23;
        //
        // lblDescription
        //
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(12, 238);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(70, 15);
        lblDescription.TabIndex = 24;
        lblDescription.Text = "Description:";
        //
        // txtDescription
        //
        txtDescription.Location = new Point(100, 235);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.ReadOnly = true;
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(520, 80);
        txtDescription.TabIndex = 25;
        //
        // lblResolutionNote
        //
        lblResolutionNote.AutoSize = true;
        lblResolutionNote.Location = new Point(12, 328);
        lblResolutionNote.Name = "lblResolutionNote";
        lblResolutionNote.Size = new Size(66, 15);
        lblResolutionNote.TabIndex = 26;
        lblResolutionNote.Text = "Resolution:";
        //
        // txtResolutionNote
        //
        txtResolutionNote.Location = new Point(100, 325);
        txtResolutionNote.Multiline = true;
        txtResolutionNote.Name = "txtResolutionNote";
        txtResolutionNote.ReadOnly = true;
        txtResolutionNote.ScrollBars = ScrollBars.Vertical;
        txtResolutionNote.Size = new Size(520, 60);
        txtResolutionNote.TabIndex = 27;
        //
        // lblNote
        //
        lblNote.AutoSize = true;
        lblNote.Location = new Point(12, 398);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(60, 15);
        lblNote.TabIndex = 28;
        lblNote.Text = "&New note:";
        //
        // txtNote
        //
        txtNote.AcceptsReturn = true;
        txtNote.Location = new Point(100, 395);
        txtNote.MaxLength = 2000;
        txtNote.Multiline = true;
        txtNote.Name = "txtNote";
        txtNote.ScrollBars = ScrollBars.Vertical;
        txtNote.Size = new Size(520, 60);
        txtNote.TabIndex = 29;
        //
        // btnStart
        //
        btnStart.Location = new Point(100, 467);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(90, 27);
        btnStart.TabIndex = 30;
        btnStart.Text = "&Start";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;
        //
        // btnResolve
        //
        btnResolve.Location = new Point(196, 467);
        btnResolve.Name = "btnResolve";
        btnResolve.Size = new Size(90, 27);
        btnResolve.TabIndex = 31;
        btnResolve.Text = "&Resolve";
        btnResolve.UseVisualStyleBackColor = true;
        btnResolve.Click += btnResolve_Click;
        //
        // btnClose
        //
        btnClose.Location = new Point(292, 467);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(90, 27);
        btnClose.TabIndex = 32;
        btnClose.Text = "&Close Ticket";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        //
        // btnReopen
        //
        btnReopen.Location = new Point(388, 467);
        btnReopen.Name = "btnReopen";
        btnReopen.Size = new Size(90, 27);
        btnReopen.TabIndex = 33;
        btnReopen.Text = "Re&open";
        btnReopen.UseVisualStyleBackColor = true;
        btnReopen.Click += btnReopen_Click;
        //
        // btnExit
        //
        btnExit.DialogResult = DialogResult.Cancel;
        btnExit.Location = new Point(535, 467);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(85, 27);
        btnExit.TabIndex = 34;
        btnExit.Text = "E&xit";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += btnExit_Click;
        //
        // TicketDetailForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnExit;
        ClientSize = new Size(640, 509);
        Controls.Add(btnExit);
        Controls.Add(btnReopen);
        Controls.Add(btnClose);
        Controls.Add(btnResolve);
        Controls.Add(btnStart);
        Controls.Add(txtNote);
        Controls.Add(lblNote);
        Controls.Add(txtResolutionNote);
        Controls.Add(lblResolutionNote);
        Controls.Add(txtDescription);
        Controls.Add(lblDescription);
        Controls.Add(txtTitle);
        Controls.Add(lblTitle);
        Controls.Add(btnRoomNumber);
        Controls.Add(txtRoom);
        Controls.Add(lblRoom);
        Controls.Add(txtClosed);
        Controls.Add(lblClosed);
        Controls.Add(txtResolved);
        Controls.Add(lblResolved);
        Controls.Add(txtDue);
        Controls.Add(lblDue);
        Controls.Add(txtCreated);
        Controls.Add(lblCreated);
        Controls.Add(txtAgent);
        Controls.Add(lblAgent);
        Controls.Add(txtPriority);
        Controls.Add(lblPriority);
        Controls.Add(txtState);
        Controls.Add(lblState);
        Controls.Add(txtType);
        Controls.Add(lblType);
        Controls.Add(btnLoad);
        Controls.Add(numTicketId);
        Controls.Add(lblTicketId);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TicketDetailForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ticket Details";
        Load += TicketDetailForm_Load;
        ((System.ComponentModel.ISupportInitialize)numTicketId).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTicketId;
    private NumericUpDown numTicketId;
    private Button btnLoad;
    private Label lblType;
    private TextBox txtType;
    private Label lblState;
    private TextBox txtState;
    private Label lblPriority;
    private TextBox txtPriority;
    private Label lblAgent;
    private TextBox txtAgent;
    private Label lblCreated;
    private TextBox txtCreated;
    private Label lblDue;
    private TextBox txtDue;
    private Label lblResolved;
    private TextBox txtResolved;
    private Label lblClosed;
    private TextBox txtClosed;
    private Label lblRoom;
    private TextBox txtRoom;
    private Button btnRoomNumber;
    private Label lblTitle;
    private TextBox txtTitle;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblResolutionNote;
    private TextBox txtResolutionNote;
    private Label lblNote;
    private TextBox txtNote;
    private Button btnStart;
    private Button btnResolve;
    private Button btnClose;
    private Button btnReopen;
    private Button btnExit;
}
