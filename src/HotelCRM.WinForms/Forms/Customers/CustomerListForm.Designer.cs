namespace HotelCRM.WinForms.Forms.Customers;

partial class CustomerListForm
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
        lblKeyword = new Label();
        txtKeyword = new TextBox();
        btnSearch = new Button();
        dgvCustomers = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colFullName = new DataGridViewTextBoxColumn();
        colPhone = new DataGridViewTextBoxColumn();
        colEmail = new DataGridViewTextBoxColumn();
        colNationality = new DataGridViewTextBoxColumn();
        colTier = new DataGridViewTextBoxColumn();
        colPoints = new DataGridViewTextBoxColumn();
        btnNew = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnHistory = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
        SuspendLayout();
        //
        // lblKeyword
        //
        lblKeyword.AutoSize = true;
        lblKeyword.Location = new Point(12, 16);
        lblKeyword.Name = "lblKeyword";
        lblKeyword.Size = new Size(56, 15);
        lblKeyword.TabIndex = 0;
        lblKeyword.Text = "&Keyword:";
        //
        // txtKeyword
        //
        txtKeyword.Location = new Point(80, 13);
        txtKeyword.MaxLength = 100;
        txtKeyword.Name = "txtKeyword";
        txtKeyword.Size = new Size(300, 23);
        txtKeyword.TabIndex = 1;
        //
        // btnSearch
        //
        btnSearch.Location = new Point(390, 12);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(80, 25);
        btnSearch.TabIndex = 2;
        btnSearch.Text = "&Search";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        //
        // dgvCustomers
        //
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.AllowUserToDeleteRows = false;
        dgvCustomers.AllowUserToResizeRows = false;
        dgvCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCustomers.Columns.AddRange(new DataGridViewColumn[] { colId, colFullName, colPhone, colEmail, colNationality, colTier, colPoints });
        dgvCustomers.Location = new Point(12, 48);
        dgvCustomers.MultiSelect = false;
        dgvCustomers.Name = "dgvCustomers";
        dgvCustomers.ReadOnly = true;
        dgvCustomers.RowHeadersVisible = false;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.Size = new Size(876, 456);
        dgvCustomers.TabIndex = 3;
        dgvCustomers.CellDoubleClick += dgvCustomers_CellDoubleClick;
        //
        // colId
        //
        colId.DataPropertyName = "Id";
        colId.FillWeight = 40F;
        colId.HeaderText = "Id";
        colId.Name = "colId";
        colId.ReadOnly = true;
        //
        // colFullName
        //
        colFullName.DataPropertyName = "FullName";
        colFullName.FillWeight = 160F;
        colFullName.HeaderText = "Full name";
        colFullName.Name = "colFullName";
        colFullName.ReadOnly = true;
        //
        // colPhone
        //
        colPhone.DataPropertyName = "Phone";
        colPhone.HeaderText = "Phone";
        colPhone.Name = "colPhone";
        colPhone.ReadOnly = true;
        //
        // colEmail
        //
        colEmail.DataPropertyName = "Email";
        colEmail.FillWeight = 160F;
        colEmail.HeaderText = "Email";
        colEmail.Name = "colEmail";
        colEmail.ReadOnly = true;
        //
        // colNationality
        //
        colNationality.DataPropertyName = "Nationality";
        colNationality.HeaderText = "Nationality";
        colNationality.Name = "colNationality";
        colNationality.ReadOnly = true;
        //
        // colTier
        //
        colTier.DataPropertyName = "Tier";
        colTier.FillWeight = 60F;
        colTier.HeaderText = "Tier";
        colTier.Name = "colTier";
        colTier.ReadOnly = true;
        //
        // colPoints
        //
        colPoints.DataPropertyName = "Points";
        colPoints.FillWeight = 60F;
        colPoints.HeaderText = "Points";
        colPoints.Name = "colPoints";
        colPoints.ReadOnly = true;
        //
        // btnNew
        //
        btnNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnNew.Location = new Point(12, 517);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(80, 27);
        btnNew.TabIndex = 4;
        btnNew.Text = "&New";
        btnNew.UseVisualStyleBackColor = true;
        btnNew.Click += btnNew_Click;
        //
        // btnEdit
        //
        btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnEdit.Location = new Point(98, 517);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(80, 27);
        btnEdit.TabIndex = 5;
        btnEdit.Text = "&Edit";
        btnEdit.UseVisualStyleBackColor = true;
        btnEdit.Click += btnEdit_Click;
        //
        // btnDelete
        //
        btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDelete.Location = new Point(184, 517);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(80, 27);
        btnDelete.TabIndex = 6;
        btnDelete.Text = "&Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        //
        // btnHistory
        //
        btnHistory.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnHistory.Location = new Point(270, 517);
        btnHistory.Name = "btnHistory";
        btnHistory.Size = new Size(80, 27);
        btnHistory.TabIndex = 7;
        btnHistory.Text = "&History";
        btnHistory.UseVisualStyleBackColor = true;
        btnHistory.Click += btnHistory_Click;
        //
        // btnClose
        //
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(808, 517);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(80, 27);
        btnClose.TabIndex = 8;
        btnClose.Text = "&Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        //
        // CustomerListForm
        //
        AcceptButton = btnSearch;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(900, 560);
        Controls.Add(btnClose);
        Controls.Add(btnHistory);
        Controls.Add(btnDelete);
        Controls.Add(btnEdit);
        Controls.Add(btnNew);
        Controls.Add(dgvCustomers);
        Controls.Add(btnSearch);
        Controls.Add(txtKeyword);
        Controls.Add(lblKeyword);
        MinimizeBox = false;
        MinimumSize = new Size(640, 400);
        Name = "CustomerListForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Customers";
        Load += CustomerListForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblKeyword;
    private TextBox txtKeyword;
    private Button btnSearch;
    private DataGridView dgvCustomers;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colFullName;
    private DataGridViewTextBoxColumn colPhone;
    private DataGridViewTextBoxColumn colEmail;
    private DataGridViewTextBoxColumn colNationality;
    private DataGridViewTextBoxColumn colTier;
    private DataGridViewTextBoxColumn colPoints;
    private Button btnNew;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnHistory;
    private Button btnClose;
}
