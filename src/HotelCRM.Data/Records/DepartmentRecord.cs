namespace HotelCRM.Data.Records;

/// <summary>One entry of <c>departments.json</c>.</summary>
public sealed class DepartmentRecord
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
