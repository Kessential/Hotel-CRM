using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC9: show the interaction history, tier and points of one customer. Content is added at stage 4.
/// </summary>
public partial class CustomerHistoryForm : Form
{
    private readonly CustomerService _customerService;

    /// <summary>Creates the form.</summary>
    /// <param name="customerService">Finds the customer and returns the history.</param>
    public CustomerHistoryForm(CustomerService customerService)
    {
        _customerService = customerService;
        InitializeComponent();
    }
}
