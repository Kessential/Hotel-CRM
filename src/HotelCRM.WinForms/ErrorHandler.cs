using HotelCRM.Domain.Common;

namespace HotelCRM.WinForms;

/// <summary>
/// The one place where the user interface handles exceptions. Every event handler of every form runs
/// its code through <see cref="Run"/>, so forms contain no try/catch of their own.
/// A <see cref="DomainException"/> is an expected business error: its message is shown as a warning.
/// Anything else is unexpected: it is written to <c>logs/error.log</c> and a general message is shown.
/// </summary>
public static class ErrorHandler
{
    private const string Caption = "Hotel CRM";
    private const string LogFolder = "logs";
    private const string LogFile = "error.log";

    /// <summary>Runs the action and handles any exception it throws.</summary>
    /// <returns>True when the action finished without an exception.</returns>
    public static bool Run(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch (Exception ex)
        {
            Handle(ex);
            return false;
        }
    }

    /// <summary>Shows a business error as a warning; logs any other error and shows a general message.</summary>
    public static void Handle(Exception ex)
    {
        if (ex is DomainException)
        {
            MessageBox.Show(ex.Message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Log(ex);
        MessageBox.Show(
            $"An unexpected error occurred. The details were written to {LogFolder}/{LogFile}.",
            Caption,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    /// <summary>
    /// Reports an error that stops the application from starting, such as a missing data file or an
    /// unknown setting. The reason is shown because the user can usually fix it.
    /// </summary>
    public static void ReportStartupFailure(Exception ex)
    {
        Log(ex);
        MessageBox.Show(
            $"The application could not start.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
            Caption,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    /// <summary>Appends the error to the log file next to the executable. A failure to write the log is ignored so the user still sees the message.</summary>
    private static void Log(Exception ex)
    {
        try
        {
            string folder = Path.Combine(AppContext.BaseDirectory, LogFolder);
            Directory.CreateDirectory(folder);
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}{Environment.NewLine}{Environment.NewLine}";
            File.AppendAllText(Path.Combine(folder, LogFile), entry);
        }
        catch (Exception logError) when (logError is IOException or UnauthorizedAccessException)
        {
        }
    }
}
