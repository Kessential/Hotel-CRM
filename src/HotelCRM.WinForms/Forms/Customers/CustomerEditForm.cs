using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC6: register a new customer or change an existing one. Content is added at stage 3.
/// </summary>
public partial class CustomerEditForm : Form
{
    private readonly CustomerService _customerService;

    /// <summary>Creates the form.</summary>
    /// <param name="customerService">Registers and updates customers.</param>
    public CustomerEditForm(CustomerService customerService)
    {
        _customerService = customerService;
        InitializeComponent();
    }
}
