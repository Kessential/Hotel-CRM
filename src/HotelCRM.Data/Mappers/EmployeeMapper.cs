using HotelCRM.Data.Records;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;

namespace HotelCRM.Data.Mappers;

/// <summary>
/// Converts between <see cref="Employee"/> and <see cref="EmployeeRecord"/>. Agents and managers share
/// one file; the <c>Role</c> field decides which subclass is rebuilt.
/// </summary>
public sealed class EmployeeMapper : IMapper<Employee, EmployeeRecord>
{
    private readonly IRepository<Department> _departments;

    /// <summary>Creates the mapper.</summary>
    /// <param name="departments">Already loaded departments, used to resolve <c>DepartmentId</c>.</param>
    public EmployeeMapper(IRepository<Department> departments)
    {
        _departments = departments;
    }

    /// <inheritdoc />
    public EmployeeRecord ToRecord(Employee entity)
    {
        return new EmployeeRecord
        {
            Id = entity.Id,
            Role = entity.Role,
            FullName = entity.FullName,
            Phone = entity.Phone,
            Email = entity.Email,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            Salt = entity.Salt,
            DepartmentId = entity.Department.Id,
        };
    }

    /// <inheritdoc />
    public Employee ToEntity(EmployeeRecord record)
    {
        Department department = _departments.GetById(record.DepartmentId)
            ?? throw new InvalidDataException($"Employee {record.Id} refers to unknown department {record.DepartmentId}.");

        return record.Role switch
        {
            EmployeeRole.Agent => Agent.Restore(
                record.Id, record.FullName, record.Phone, record.Email,
                record.Username, record.PasswordHash, record.Salt, department),
            EmployeeRole.Manager => Manager.Restore(
                record.Id, record.FullName, record.Phone, record.Email,
                record.Username, record.PasswordHash, record.Salt, department),
            _ => throw new InvalidDataException($"Employee {record.Id} has unknown role '{record.Role}'."),
        };
    }
}
