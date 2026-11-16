using HotelCRM.Data.Records;
using HotelCRM.Domain.Staff;

namespace HotelCRM.Data.Mappers;

/// <summary>Converts between <see cref="Department"/> and <see cref="DepartmentRecord"/>.</summary>
public sealed class DepartmentMapper : IMapper<Department, DepartmentRecord>
{
    /// <inheritdoc />
    public DepartmentRecord ToRecord(Department entity)
    {
        return new DepartmentRecord
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
        };
    }

    /// <inheritdoc />
    public Department ToEntity(DepartmentRecord record)
    {
        return Department.Restore(record.Id, record.Code, record.Name);
    }
}
