using HotelCRM.Domain.Common;

namespace HotelCRM.Data.Records;

/// <summary>
/// One entry of <c>employees.json</c>. <see cref="Role"/> (<c>Agent</c> or <c>Manager</c>) tells the
/// mapper which subclass to rebuild. The department is stored by Id.
/// </summary>
public sealed class EmployeeRecord
{
    public int Id { get; set; }

    public EmployeeRole Role { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public int DepartmentId { get; set; }
}
