using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC6: search customers and delete one. Opens the edit and history forms. Content is added at stage 3.
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
    }
}
