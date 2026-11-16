using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;

namespace HotelCRM.Tests.Tickets;

/// <summary>Fixed values and small builders shared by the ticket tests.</summary>
internal static class TicketTestData
{
    /// <summary>The fixed "now" of every ticket test.</summary>
    public static readonly DateTime Now = new(2026, 11, 16, 9, 0, 0);

    /// <summary>Profile of a Standard customer: no faster handling, no priority boost.</summary>
    public static readonly CustomerSlaProfile StandardProfile = new(SlaFactor: 1.0, PriorityBoost: 0);

    public const int CustomerId = 7;
    public const int MaxActive = 5;
    public const int ReopenDays = 7;

    public static Department NewDepartment()
    {
        return new Department(1, "HOUSE", "Housekeeping");
    }

    public static Agent NewAgent(int id, Department department)
    {
        return new Agent(id, $"Agent {id}", "0901000002", $"agent{id}@hotel.example", $"agent{id}", "hash", "salt", department);
    }

    public static Manager NewManager(int id, Department department)
    {
        return new Manager(id, $"Manager {id}", "0901000001", $"manager{id}@hotel.example", $"manager{id}", "hash", "salt", department);
    }

    /// <summary>A new Open, unassigned service request with Medium priority.</summary>
    public static SupportTicket NewTicket()
    {
        return new ServiceRequestTicket(1, CustomerId, "Extra towels", "Two bath towels, please.", TicketPriority.Medium, StandardProfile, Now);
    }

    /// <summary>A ticket that has been assigned, started and resolved at <see cref="Now"/>.</summary>
    public static SupportTicket NewResolvedTicket()
    {
        SupportTicket ticket = NewTicket();
        ticket.AssignTo(NewAgent(2, NewDepartment()), activeCount: 0, MaxActive);
        ticket.Start(Now);
        ticket.Resolve("Towels delivered.", Now);
        return ticket;
    }
}
