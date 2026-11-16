using HotelCRM.Domain.Staff;

namespace HotelCRM.Domain.Common;

/// <summary>
/// Base class of the two actors of the system. Holds the login account and the department,
/// and declares the permission checks of rule 11 that <see cref="Agent"/> and <see cref="Manager"/> override.
/// </summary>
public abstract class Employee : Person
{
    /// <summary>Creates an employee account.</summary>
    /// <exception cref="ValidationException">
    /// Name, phone or email is invalid (see <see cref="Person"/>), or username, password hash, salt or department is missing.
    /// </exception>
    protected Employee(
        int id,
        string fullName,
        string phone,
        string email,
        string username,
        string passwordHash,
        string salt,
        Department department)
        : base(id, fullName, phone, email)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ValidationException("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash) || string.IsNullOrWhiteSpace(salt))
        {
            throw new ValidationException("Password hash and salt are required.");
        }

        Username = username.Trim();
        PasswordHash = passwordHash;
        Salt = salt;
        Department = department ?? throw new ValidationException("Department is required.");
    }

    /// <summary>Login name; compared case-insensitively.</summary>
    public string Username { get; }

    /// <summary>PBKDF2-SHA256 hash of the password, Base64 encoded. The password itself is never stored.</summary>
    public string PasswordHash { get; }

    /// <summary>Random 16-byte salt of this account, Base64 encoded.</summary>
    public string Salt { get; }

    /// <summary>Department the employee works in.</summary>
    public Department Department { get; }

    /// <summary>Role of the employee; fixed by the concrete class.</summary>
    public abstract EmployeeRole Role { get; }

    /// <summary>Whether this employee may assign or reassign tickets (rule 11: managers only).</summary>
    public abstract bool CanAssign();

    /// <summary>Whether this employee may close tickets (rule 11: managers only).</summary>
    public abstract bool CanClose();

    /// <summary>Whether this employee may reopen tickets (rule 11: managers only).</summary>
    public abstract bool CanReopen();

    /// <summary>Rule 11: stops the operation when this employee may not assign tickets.</summary>
    /// <exception cref="BusinessRuleException"><see cref="CanAssign"/> returns false.</exception>
    public void EnsureCanAssign()
    {
        if (!CanAssign())
        {
            throw new BusinessRuleException("Only a manager can assign tickets.");
        }
    }

    /// <summary>Rule 11: stops the operation when this employee may not close tickets.</summary>
    /// <exception cref="BusinessRuleException"><see cref="CanClose"/> returns false.</exception>
    public void EnsureCanClose()
    {
        if (!CanClose())
        {
            throw new BusinessRuleException("Only a manager can close tickets.");
        }
    }

    /// <summary>Rule 11: stops the operation when this employee may not reopen tickets.</summary>
    /// <exception cref="BusinessRuleException"><see cref="CanReopen"/> returns false.</exception>
    public void EnsureCanReopen()
    {
        if (!CanReopen())
        {
            throw new BusinessRuleException("Only a manager can reopen tickets.");
        }
    }

    /// <summary>
    /// Decides the outcome of a login attempt. The service looks the account up and verifies the
    /// password; this method turns the two results into either the logged-in employee or an error.
    /// </summary>
    /// <param name="account">Account found for the entered username, or null when there is none.</param>
    /// <param name="passwordMatches">Whether the entered password matches the stored hash.</param>
    /// <returns>The employee who is now logged in.</returns>
    /// <exception cref="BusinessRuleException">The account does not exist or the password is wrong.</exception>
    public static Employee EnsureLoginValid(Employee? account, bool passwordMatches)
    {
        if (account is null || !passwordMatches)
        {
            throw new BusinessRuleException("Invalid username or password.");
        }

        return account;
    }
}
