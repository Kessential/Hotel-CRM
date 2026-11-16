using HotelCRM.Data.Records;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;
using HotelCRM.Domain.Staff;
using HotelCRM.Domain.Tickets;
using HotelCRM.Domain.Tickets.States;

namespace HotelCRM.Data.Mappers;

/// <summary>
/// Converts between <see cref="SupportTicket"/> and <see cref="TicketRecord"/>. The three ticket kinds
/// share one file; the <c>Type</c> field decides which subclass is rebuilt, and the state is rebuilt
/// from its name with <see cref="TicketState.FromName"/>. Adding a ticket kind means adding one line
/// to the switch in <see cref="ToEntity"/>.
/// </summary>
public sealed class TicketMapper : IMapper<SupportTicket, TicketRecord>
{
    private readonly IRepository<Employee> _employees;

    /// <summary>Creates the mapper.</summary>
    /// <param name="employees">Already loaded employees, used to resolve <c>AssignedAgentId</c>.</param>
    public TicketMapper(IRepository<Employee> employees)
    {
        _employees = employees;
    }

    /// <inheritdoc />
    public TicketRecord ToRecord(SupportTicket entity)
    {
        return new TicketRecord
        {
            Id = entity.Id,
            Type = entity.TypeName,
            CustomerId = entity.CustomerId,
            Title = entity.Title,
            Description = entity.Description,
            Priority = entity.Priority,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            UpdatedAt = entity.UpdatedAt,
            State = entity.State.Name,
            AssignedAgentId = entity.AssignedAgent?.Id,
            ResolvedAt = entity.ResolvedAt,
            ResolutionNote = entity.ResolutionNote,
            ClosedAt = entity.ClosedAt,
            RoomId = (entity as MaintenanceTicket)?.RoomId,
        };
    }

    /// <inheritdoc />
    public SupportTicket ToEntity(TicketRecord record)
    {
        TicketSnapshot snapshot = new(
            record.Id,
            record.CustomerId,
            record.Title,
            record.Description,
            record.Priority,
            record.CreatedAt,
            record.DueAt,
            record.UpdatedAt,
            TicketState.FromName(record.State),
            FindAgent(record),
            record.ResolvedAt,
            record.ResolutionNote,
            record.ClosedAt);

        return record.Type switch
        {
            ComplaintTicket.TypeKey => ComplaintTicket.Restore(snapshot),
            ServiceRequestTicket.TypeKey => ServiceRequestTicket.Restore(snapshot),
            MaintenanceTicket.TypeKey => MaintenanceTicket.Restore(
                snapshot,
                record.RoomId ?? throw new InvalidDataException($"Maintenance ticket {record.Id} has no room.")),
            _ => throw new InvalidDataException($"Ticket {record.Id} has unknown type '{record.Type}'."),
        };
    }

    private Agent? FindAgent(TicketRecord record)
    {
        if (record.AssignedAgentId is not int agentId)
        {
            return null;
        }

        return _employees.GetById(agentId) as Agent
            ?? throw new InvalidDataException($"Ticket {record.Id} refers to unknown agent {agentId}.");
    }
}
