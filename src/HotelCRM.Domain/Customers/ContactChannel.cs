namespace HotelCRM.Domain.Customers;

/// <summary>How an interaction with the customer took place.</summary>
public enum ContactChannel
{
    /// <summary>In person at the front desk, for example check-in and check-out.</summary>
    FrontDesk,

    /// <summary>By phone.</summary>
    Phone,

    /// <summary>By email.</summary>
    Email,

    /// <summary>Recorded automatically by the system, for example when a ticket changes state.</summary>
    System,
}
