using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Customers;

/// <summary>
/// UC8: record the satisfaction score a customer gives for a resolved or closed ticket. Content is added at stage 4.
/// </summary>
public partial class SurveyForm : Form
{
    private readonly SurveyService _surveyService;
    private readonly CustomerService _customerService;

    /// <summary>Creates the form.</summary>
    /// <param name="surveyService">Lists the tickets that can be surveyed and saves the survey.</param>
    /// <param name="customerService">Finds the customer whose ticket is surveyed.</param>
    public SurveyForm(SurveyService surveyService, CustomerService customerService)
    {
        _surveyService = surveyService;
        _customerService = customerService;
        InitializeComponent();
    }
}
