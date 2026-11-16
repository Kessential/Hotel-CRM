using HotelCRM.Business.Services;

namespace HotelCRM.WinForms.Forms;

/// <summary>
/// UC1: log in. Closes with <see cref="DialogResult.OK"/> once <see cref="AuthService.Login"/> succeeds.
/// Template for every other form: the services arrive through the constructor, the layout lives in
/// the Designer file, and every event handler runs its code inside <see cref="ErrorHandler.Run"/>.
/// </summary>
public partial class LoginForm : Form
{
    private readonly AuthService _authService;

    /// <summary>Creates the form.</summary>
    /// <param name="authService">Service that checks the credentials and starts the session.</param>
    public LoginForm(AuthService authService)
    {
        _authService = authService;
        InitializeComponent();
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        bool loggedIn = ErrorHandler.Run(() =>
        {
            _authService.Login(txtUsername.Text, txtPassword.Text);
            DialogResult = DialogResult.OK;
            Close();
        });

        if (!loggedIn)
        {
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }

    private void btnExit_Click(object? sender, EventArgs e)
    {
        ErrorHandler.Run(() =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        });
    }
}
