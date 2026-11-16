using HotelCRM.Business.Assignment;
using HotelCRM.Business.Factories;
using HotelCRM.Business.Services;
using HotelCRM.Data;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;
using HotelCRM.Tests.Fakes;

namespace HotelCRM.Tests.Tickets;

/// <summary>
/// Rule 5 (maintenance room must be in an active stay) and rule 11 (permissions), checked through
/// <see cref="TicketService"/> with in-memory repositories and hand-written fakes.
/// </summary>
public class TicketServiceTests
{
    private const int RoomId = 305;

    private static readonly DateTime _now = TicketTestData.Now;

    private readonly InMemoryRepository<SupportTicket> _tickets = new();
    private readonly InMemoryRepository<Employee> _employees = new();
    private readonly FakeCustomerDirectory _customers = new();
    private readonly FakeStayLookup _stays = new();
    private readonly Manager _manager;
    private readonly Agent _agent;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        Department department = TicketTestData.NewDepartment();
        _manager = TicketTestData.NewManager(1, department);
        _agent = TicketTestData.NewAgent(2, department);
        _employees.Save(_manager);
        _employees.Save(_agent);
        _customers.Add(TicketTestData.CustomerId, TicketTestData.StandardProfile);

        Dictionary<string, TicketFactory> factories = new()
        {
            [ComplaintTicket.TypeKey] = new ComplaintTicketFactory(),
            [ServiceRequestTicket.TypeKey] = new ServiceRequestTicketFactory(),
            [MaintenanceTicket.TypeKey] = new MaintenanceTicketFactory(),
        };

        _service = new TicketService(
            _tickets,
            _employees,
            _customers,
            _stays,
            new FirstCandidateStrategy(),
            factories,
            Array.Empty<ITicketObserver>(),
            new BusinessSettings(TicketTestData.MaxActive, TicketTestData.ReopenDays));
    }

    [Fact]
    public void CreateTicket_MaintenanceRoomNotInActiveStay_ThrowsBusinessRule()
    {
        // Arrange: the customer exists but is not staying in room 305 (the fake has no active stay).

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => _service.CreateTicket(
            MaintenanceTicket.TypeKey,
            TicketTestData.CustomerId,
            "Air conditioner broken",
            "It does not cool the room.",
            TicketPriority.High,
            RoomId,
            _now));
    }

    [Fact]
    public void Close_ByAgent_ThrowsBusinessRule()
    {
        // Arrange: a resolved ticket; closing is reserved for managers.
        SupportTicket ticket = _service.CreateTicket(
            ServiceRequestTicket.TypeKey,
            TicketTestData.CustomerId,
            "Extra towels",
            "Two bath towels, please.",
            TicketPriority.Medium,
            roomId: null,
            _now);
        _service.AssignManually(_manager, ticket.Id, _agent.Id);
        _service.Start(_agent, ticket.Id, _now);
        _service.Resolve(_agent, ticket.Id, "Towels delivered.", _now);

        // Act + Assert
        Employee actor = _agent;
        Assert.Throws<BusinessRuleException>(() => _service.Close(actor, ticket.Id, _now));
    }

    /// <summary>
    /// Smallest possible strategy. These tests never assign automatically; the service only needs
    /// some strategy to be constructed, and using a local one keeps the ticket tests independent of
    /// the strategies written by the other share.
    /// </summary>
    private sealed class FirstCandidateStrategy : IAssignmentStrategy
    {
        public Agent? Pick(string departmentCode, IReadOnlyList<AgentWorkload> candidates)
        {
            return candidates.Count == 0 ? null : candidates[0].Agent;
        }
    }
}
