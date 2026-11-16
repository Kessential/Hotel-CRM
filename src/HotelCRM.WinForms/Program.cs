namespace HotelCRM.WinForms;

/// <summary>
/// Application entry point. Stage 1 module 4 turns this into the composition root
/// (configuration, repositories, observers, strategies, factories, services, forms).
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
    }
}
