using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Staff;

/// <summary>
/// An employee who supervises the agents. A manager may assign, reassign, close and reopen tickets
/// and read reports (rule 11), but does not start or resolve tickets.
/// </summary>
public sealed class Manager : Employee
{
    /// <summary>Creates a manager.</summary>
    /// <exception cref="ValidationException">See <see cref="Employee"/>.</exception>
    public Manager(
        int id,
        string fullName,
        string phone,
        string email,
        string username,
        string passwordHash,
        string salt,
        Department department)
        : base(id, fullName, phone, email, username, passwordHash, salt, department)
    {
    }

    /// <inheritdoc />
    public override EmployeeRole Role => EmployeeRole.Manager;

    /// <inheritdoc />
    public override bool CanAssign()
    {
        return true;
    }

    /// <inheritdoc />
    public override bool CanClose()
    {
        return true;
    }

    /// <inheritdoc />
    public override bool CanReopen()
    {
        return true;
    }

    /// <summary>Rebuilds a manager from stored data.</summary>
    internal static Manager Restore(
        int id,
        string fullName,
        string phone,
        string email,
        string username,
        string passwordHash,
        string salt,
        Department department)
    {
        return new Manager(id, fullName, phone, email, username, passwordHash, salt, department);
    }
}
