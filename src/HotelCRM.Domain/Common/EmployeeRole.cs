namespace HotelCRM.Domain.Common;

/// <summary>Role of an employee; decides which menus and ticket actions are available (rule 11).</summary>
public enum EmployeeRole
{
    /// <summary>Handles the tickets assigned to them.</summary>
    Agent,

    /// <summary>Assigns, closes and reopens tickets and reads reports.</summary>
    Manager,
}
