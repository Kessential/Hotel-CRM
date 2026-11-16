using HotelCRM.Business.Security;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Business.Services;

/// <summary>UC1: log in and log out.</summary>
public sealed class AuthService
{
    private readonly IRepository<Employee> _employees;

    /// <summary>Creates the service.</summary>
    /// <param name="employees">Repository of agents and managers.</param>
    public AuthService(IRepository<Employee> employees)
    {
        _employees = employees;
    }

    /// <summary>
    /// Logs an employee in. Looks the account up by username (case-insensitive), verifies the password
    /// with <see cref="PasswordHasher"/>, lets <see cref="Employee.EnsureLoginValid"/> decide, and then
    /// starts the <see cref="UserSession"/>.
    /// </summary>
    /// <returns>The employee who is now logged in.</returns>
    /// <exception cref="BusinessRuleException">The username does not exist or the password is wrong.</exception>
    public Employee Login(string username, string password)
    {
        string name = (username ?? string.Empty).Trim();
        Employee? account = _employees.GetAll()
            .FirstOrDefault(e => string.Equals(e.Username, name, StringComparison.OrdinalIgnoreCase));
        bool passwordMatches = account is not null
            && PasswordHasher.Verify(password ?? string.Empty, account.PasswordHash, account.Salt);

        Employee employee = Employee.EnsureLoginValid(account, passwordMatches);
        UserSession.Start(employee);
        return employee;
    }

    /// <summary>Ends the session of the logged-in employee.</summary>
    public void Logout()
    {
        UserSession.End();
    }
}
