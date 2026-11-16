using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms.Reports;

/// <summary>
/// UC10: agent performance and department reports for the manager. Content is added at stage 4.
/// </summary>
public partial class ReportForm : Form
{
    private readonly ReportService _reportService;

    /// <summary>Creates the form.</summary>
    /// <param name="reportService">Computes the two reports.</param>
    public ReportForm(ReportService reportService)
    {
        _reportService = reportService;
        InitializeComponent();
    }
}
