namespace HotelCRM.WinForms.Forms.Customers;

partial class CustomerEditForm
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
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblIdentityNumber = new Label();
        txtIdentityNumber = new TextBox();
        lblNationality = new Label();
        txtNationality = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        SuspendLayout();
        //
        // lblFullName
        //
        lblFullName.AutoSize = true;
        lblFullName.Location = new Point(24, 27);
        lblFullName.Name = "lblFullName";
        lblFullName.Size = new Size(64, 15);
        lblFullName.TabIndex = 0;
        lblFullName.Text = "&Full name:";
        //
        // txtFullName
        //
        txtFullName.Location = new Point(150, 24);
        txtFullName.MaxLength = 100;
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(340, 23);
        txtFullName.TabIndex = 1;
        //
        // lblPhone
        //
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(24, 62);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(44, 15);
        lblPhone.TabIndex = 2;
        lblPhone.Text = "&Phone:";
        //
        // txtPhone
        //
        txtPhone.Location = new Point(150, 59);
        txtPhone.MaxLength = 10;
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(160, 23);
        txtPhone.TabIndex = 3;
        //
        // lblEmail
        //
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(24, 97);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 4;
        lblEmail.Text = "&Email:";
        //
        // txtEmail
        //
        txtEmail.Location = new Point(150, 94);
        txtEmail.MaxLength = 100;
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(340, 23);
        txtEmail.TabIndex = 5;
        //
        // lblIdentityNumber
        //
        lblIdentityNumber.AutoSize = true;
        lblIdentityNumber.Location = new Point(24, 132);
        lblIdentityNumber.Name = "lblIdentityNumber";
        lblIdentityNumber.Size = new Size(98, 15);
        lblIdentityNumber.TabIndex = 6;
        lblIdentityNumber.Text = "&Identity number:";
        //
        // txtIdentityNumber
        //
        txtIdentityNumber.Location = new Point(150, 129);
        txtIdentityNumber.MaxLength = 30;
        txtIdentityNumber.Name = "txtIdentityNumber";
        txtIdentityNumber.Size = new Size(200, 23);
        txtIdentityNumber.TabIndex = 7;
        //
        // lblNationality
        //
        lblNationality.AutoSize = true;
        lblNationality.Location = new Point(24, 167);
        lblNationality.Name = "lblNationality";
        lblNationality.Size = new Size(68, 15);
        lblNationality.TabIndex = 8;
        lblNationality.Text = "N&ationality:";
        //
        // txtNationality
        //
        txtNationality.Location = new Point(150, 164);
        txtNationality.MaxLength = 50;
        txtNationality.Name = "txtNationality";
        txtNationality.Size = new Size(200, 23);
        txtNationality.TabIndex = 9;
        //
        // lblNotes
        //
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(24, 202);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 10;
        lblNotes.Text = "N&otes:";
        //
        // txtNotes
        //
        txtNotes.Location = new Point(150, 199);
        txtNotes.MaxLength = 500;
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(340, 140);
        txtNotes.TabIndex = 11;
        //
        // btnSave
        //
        btnSave.Location = new Point(324, 368);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(80, 27);
        btnSave.TabIndex = 12;
        btnSave.Text = "&Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(410, 368);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 27);
        btnCancel.TabIndex = 13;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        //
        // CustomerEditForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(520, 420);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(txtNationality);
        Controls.Add(lblNationality);
        Controls.Add(txtIdentityNumber);
        Controls.Add(lblIdentityNumber);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtFullName);
        Controls.Add(lblFullName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CustomerEditForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Customer";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFullName;
    private TextBox txtFullName;
    private Label lblPhone;
    private TextBox txtPhone;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblIdentityNumber;
    private TextBox txtIdentityNumber;
    private Label lblNationality;
    private TextBox txtNationality;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnCancel;
}
