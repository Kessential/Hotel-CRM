namespace HotelCRM.WinForms.Forms.Tickets;

partial class TicketListForm
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
        lblState = new Label();
        cboState = new ComboBox();
        lblPriority = new Label();
        cboPriority = new ComboBox();
        lblType = new Label();
        cboType = new ComboBox();
        lblAgent = new Label();
        cboAgent = new ComboBox();
        lblKeyword = new Label();
        txtKeyword = new TextBox();
        btnSearch = new Button();
        btnClear = new Button();
        lblCount = new Label();
        dgvTickets = new DataGridView();
        btnNew = new Button();
        btnDetails = new Button();
        btnAutoAssign = new Button();
        lblAssignAgent = new Label();
        cboAssignAgent = new ComboBox();
        btnAssign = new Button();
        btnExit = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvTickets).BeginInit();
        SuspendLayout();
        //
        // lblState
        //
        lblState.AutoSize = true;
        lblState.Location = new Point(12, 18);
        lblState.Name = "lblState";
        lblState.Size = new Size(36, 15);
        lblState.TabIndex = 0;
        lblState.Text = "&State:";
        //
        // cboState
        //
        cboState.DropDownStyle = ComboBoxStyle.DropDownList;
        cboState.Location = new Point(76, 15);
        cboState.Name = "cboState";
        cboState.Size = new Size(120, 23);
        cboState.TabIndex = 1;
        //
        // lblPriority
        //
        lblPriority.AutoSize = true;
        lblPriority.Location = new Point(212, 18);
        lblPriority.Name = "lblPriority";
        lblPriority.Size = new Size(48, 15);
        lblPriority.TabIndex = 2;
        lblPriority.Text = "&Priority:";
        //
        // cboPriority
        //
        cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPriority.Location = new Point(270, 15);
        cboPriority.Name = "cboPriority";
        cboPriority.Size = new Size(110, 23);
        cboPriority.TabIndex = 3;
        //
        // lblType
        //
        lblType.AutoSize = true;
        lblType.Location = new Point(396, 18);
        lblType.Name = "lblType";
        lblType.Size = new Size(34, 15);
        lblType.TabIndex = 4;
        lblType.Text = "&Type:";
        //
        // cboType
        //
        cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboType.Location = new Point(440, 15);
        cboType.Name = "cboType";
        cboType.Size = new Size(130, 23);
        cboType.TabIndex = 5;
        //
        // lblAgent
        //
        lblAgent.AutoSize = true;
        lblAgent.Location = new Point(586, 18);
        lblAgent.Name = "lblAgent";
        lblAgent.Size = new Size(42, 15);
        lblAgent.TabIndex = 6;
        lblAgent.Text = "&Agent:";
        //
        // cboAgent
        //
        cboAgent.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAgent.Location = new Point(638, 15);
        cboAgent.Name = "cboAgent";
        cboAgent.Size = new Size(170, 23);
        cboAgent.TabIndex = 7;
        //
        // lblKeyword
        //
        lblKeyword.AutoSize = true;
        lblKeyword.Location = new Point(12, 50);
        lblKeyword.Name = "lblKeyword";
        lblKeyword.Size = new Size(56, 15);
        lblKeyword.TabIndex = 8;
        lblKeyword.Text = "&Keyword:";
        //
        // txtKeyword
        //
        txtKeyword.Location = new Point(76, 47);
        txtKeyword.MaxLength = 100;
        txtKeyword.Name = "txtKeyword";
        txtKeyword.Size = new Size(304, 23);
        txtKeyword.TabIndex = 9;
        //
        // btnSearch
        //
        btnSearch.Location = new Point(396, 45);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(85, 27);
        btnSearch.TabIndex = 10;
        btnSearch.Text = "Sea&rch";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        //
        // btnClear
        //
        btnClear.Location = new Point(487, 45);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(85, 27);
        btnClear.TabIndex = 11;
        btnClear.Text = "C&lear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        //
        // lblCount
        //
        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Location = new Point(788, 51);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(160, 15);
        lblCount.TabIndex = 12;
        lblCount.Text = "0 ticket(s)";
        lblCount.TextAlign = ContentAlignment.MiddleRight;
        //
        // dgvTickets
        //
        dgvTickets.AllowUserToAddRows = false;
        dgvTickets.AllowUserToDeleteRows = false;
        dgvTickets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTickets.Location = new Point(12, 82);
        dgvTickets.MultiSelect = false;
        dgvTickets.Name = "dgvTickets";
        dgvTickets.ReadOnly = true;
        dgvTickets.RowHeadersVisible = false;
        dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTickets.Size = new Size(936, 460);
        dgvTickets.TabIndex = 13;
        dgvTickets.CellDoubleClick += dgvTickets_CellDoubleClick;
        //
        // btnNew
        //
        btnNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnNew.Location = new Point(12, 556);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(95, 27);
        btnNew.TabIndex = 14;
        btnNew.Text = "&New Ticket";
        btnNew.UseVisualStyleBackColor = true;
        btnNew.Click += btnNew_Click;
        //
        // btnDetails
        //
        btnDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDetails.Location = new Point(113, 556);
        btnDetails.Name = "btnDetails";
        btnDetails.Size = new Size(95, 27);
        btnDetails.TabIndex = 15;
        btnDetails.Text = "&Details";
        btnDetails.UseVisualStyleBackColor = true;
        btnDetails.Click += btnDetails_Click;
        //
        // btnAutoAssign
        //
        btnAutoAssign.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnAutoAssign.Location = new Point(300, 556);
        btnAutoAssign.Name = "btnAutoAssign";
        btnAutoAssign.Size = new Size(100, 27);
        btnAutoAssign.TabIndex = 16;
        btnAutoAssign.Text = "A&uto Assign";
        btnAutoAssign.UseVisualStyleBackColor = true;
        btnAutoAssign.Click += btnAutoAssign_Click;
        //
        // lblAssignAgent
        //
        lblAssignAgent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblAssignAgent.AutoSize = true;
        lblAssignAgent.Location = new Point(420, 562);
        lblAssignAgent.Name = "lblAssignAgent";
        lblAssignAgent.Size = new Size(58, 15);
        lblAssignAgent.TabIndex = 17;
        lblAssignAgent.Text = "Assign to:";
        //
        // cboAssignAgent
        //
        cboAssignAgent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        cboAssignAgent.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAssignAgent.Location = new Point(488, 558);
        cboAssignAgent.Name = "cboAssignAgent";
        cboAssignAgent.Size = new Size(170, 23);
        cboAssignAgent.TabIndex = 18;
        //
        // btnAssign
        //
        btnAssign.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnAssign.Location = new Point(664, 556);
        btnAssign.Name = "btnAssign";
        btnAssign.Size = new Size(85, 27);
        btnAssign.TabIndex = 19;
        btnAssign.Text = "Assi&gn";
        btnAssign.UseVisualStyleBackColor = true;
        btnAssign.Click += btnAssign_Click;
        //
        // btnExit
        //
        btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnExit.DialogResult = DialogResult.Cancel;
        btnExit.Location = new Point(863, 556);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(85, 27);
        btnExit.TabIndex = 20;
        btnExit.Text = "E&xit";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += btnExit_Click;
        //
        // TicketListForm
        //
        AcceptButton = btnSearch;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnExit;
        ClientSize = new Size(960, 600);
        Controls.Add(btnExit);
        Controls.Add(btnAssign);
        Controls.Add(cboAssignAgent);
        Controls.Add(lblAssignAgent);
        Controls.Add(btnAutoAssign);
        Controls.Add(btnDetails);
        Controls.Add(btnNew);
        Controls.Add(dgvTickets);
        Controls.Add(lblCount);
        Controls.Add(btnClear);
        Controls.Add(btnSearch);
        Controls.Add(txtKeyword);
        Controls.Add(lblKeyword);
        Controls.Add(cboAgent);
        Controls.Add(lblAgent);
        Controls.Add(cboType);
        Controls.Add(lblType);
        Controls.Add(cboPriority);
        Controls.Add(lblPriority);
        Controls.Add(cboState);
        Controls.Add(lblState);
        MinimizeBox = false;
        MinimumSize = new Size(820, 400);
        Name = "TicketListForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tickets";
        Load += TicketListForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvTickets).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblState;
    private ComboBox cboState;
    private Label lblPriority;
    private ComboBox cboPriority;
    private Label lblType;
    private ComboBox cboType;
    private Label lblAgent;
    private ComboBox cboAgent;
    private Label lblKeyword;
    private TextBox txtKeyword;
    private Button btnSearch;
    private Button btnClear;
    private Label lblCount;
    private DataGridView dgvTickets;
    private Button btnNew;
    private Button btnDetails;
    private Button btnAutoAssign;
    private Label lblAssignAgent;
    private ComboBox cboAssignAgent;
    private Button btnAssign;
    private Button btnExit;
}
