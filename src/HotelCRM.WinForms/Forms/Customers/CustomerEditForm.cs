using HotelCRM.Business.Services;
using HotelCRM.Domain.Customers;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC6: register a new customer or change an existing one. Opened empty it registers a new customer;
/// after <see cref="LoadCustomer"/> it edits that customer. Closes with <see cref="DialogResult.OK"/>
/// once the customer is saved.
/// </summary>
public partial class CustomerEditForm : Form
{
    private readonly CustomerService _customerService;
    private int? _customerId;

    /// <summary>Creates the form.</summary>
    /// <param name="customerService">Registers and updates customers.</param>
    public CustomerEditForm(CustomerService customerService)
    {
        _customerService = customerService;
        InitializeComponent();
    }

    /// <summary>Switches the form to editing an existing customer and fills in the fields. Call before showing the form.</summary>
    /// <param name="customerId">Id of the customer to edit.</param>
    /// <exception cref="Domain.Common.BusinessRuleException">The customer does not exist.</exception>
    public void LoadCustomer(int customerId)
    {
        Customer customer = _customerService.GetById(customerId);
        _customerId = customer.Id;
        txtFullName.Text = customer.FullName;
        txtPhone.Text = customer.Phone;
        txtEmail.Text = customer.Email;
        txtIdentityNumber.Text = customer.IdentityNumber;
        txtNationality.Text = customer.Nationality;
        txtNotes.Text = customer.Notes;
        Text = $"Edit Customer - {customer.FullName}";
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            if (_customerId is int customerId)
            {
                _customerService.Update(
                    customerId,
                    txtFullName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtIdentityNumber.Text,
                    txtNationality.Text,
                    txtNotes.Text);
            }
            else
            {
                _customerService.Register(
                    txtFullName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtIdentityNumber.Text,
                    txtNationality.Text,
                    txtNotes.Text);
            }

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
}
