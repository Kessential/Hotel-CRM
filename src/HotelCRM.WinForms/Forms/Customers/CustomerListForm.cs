using HotelCRM.Business.Services;
using HotelCRM.Domain.Customers;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC6: search customers and delete one. Opens the edit form to register or change a customer and
/// the history form to see one customer's interactions.
/// </summary>
public partial class CustomerListForm : Form
{
    private readonly CustomerService _customerService;

    /// <summary>Creates the form.</summary>
    /// <param name="customerService">Searches and deletes customers.</param>
    public CustomerListForm(CustomerService customerService)
    {
        _customerService = customerService;
        InitializeComponent();
        dgvCustomers.AutoGenerateColumns = false;
    }

    private void CustomerListForm_Load(object? sender, EventArgs e)
    {
        ErrorHandler.Run(LoadCustomers);
    }

    private void btnSearch_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(LoadCustomers);
    }

    private void btnNew_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            using CustomerEditForm form = new(_customerService);
            form.ShowDialog(this);
            LoadCustomers();
        });
    }

    private void btnEdit_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(EditSelectedCustomer);
    }

    private void dgvCustomers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            ErrorHandler.Run(EditSelectedCustomer);
        }
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            CustomerRow? row = SelectedRow();
            if (row is null)
            {
                return;
            }

            DialogResult answer = MessageBox.Show(
                $"Delete customer {row.FullName}?",
                Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            _customerService.Delete(row.Id);
            LoadCustomers();
        });
    }

    private void btnHistory_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (SelectedRow() is null)
            {
                return;
            }

            // The history form gets its content and a way to receive the customer Id at stage 4.
            using CustomerHistoryForm form = new(_customerService);
            form.ShowDialog(this);
            LoadCustomers();
        });
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(Close);
    }

    private void EditSelectedCustomer()
    {
        CustomerRow? row = SelectedRow();
        if (row is null)
        {
            return;
        }

        using CustomerEditForm form = new(_customerService);
        form.LoadCustomer(row.Id);
        form.ShowDialog(this);
        LoadCustomers();
    }

    /// <summary>Fills the grid with the customers matching the keyword box.</summary>
    private void LoadCustomers()
    {
        dgvCustomers.DataSource = _customerService.Search(txtKeyword.Text)
            .Select(CustomerRow.From)
            .ToList();
    }

    /// <summary>Returns the selected row, or tells the user to select one and returns null.</summary>
    private CustomerRow? SelectedRow()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is CustomerRow row)
        {
            return row;
        }

        MessageBox.Show("Select a customer first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    /// <summary>One line of the grid.</summary>
    private sealed record CustomerRow(
        int Id,
        string FullName,
        string Phone,
        string Email,
        string Nationality,
        string Tier,
        int Points)
    {
        public static CustomerRow From(Customer customer)
        {
            return new CustomerRow(
                customer.Id,
                customer.FullName,
                customer.Phone,
                customer.Email,
                customer.Nationality,
                customer.Tier.Name,
                customer.Points);
        }
    }
}
