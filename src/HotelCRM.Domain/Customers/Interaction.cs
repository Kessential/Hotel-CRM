using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// One contact with a customer (table <c>interactions</c>). Immutable once created. Refers to the customer,
/// the employee and the ticket by Id, so it is stored and read on its own instead of inside the customer.
/// </summary>
public sealed class Interaction : IEntity
{
    /// <summary>Records an interaction.</summary>
    /// <param name="id">Id assigned by the interaction repository.</param>
    /// <param name="customerId">Customer the interaction is about.</param>
    /// <param name="channel">How it took place.</param>
    /// <param name="summary">What happened; required.</param>
    /// <param name="occurredAt">When it happened (the caller's "now").</param>
    /// <param name="employeeId">
    /// Employee who recorded it. Null exactly when <paramref name="channel"/> is
    /// <see cref="ContactChannel.System"/>, that is when an observer records it automatically.
    /// </param>
    /// <param name="ticketId">Ticket the interaction comes from (Q12), or null.</param>
    /// <exception cref="ValidationException">
    /// The summary is empty, or the employee is missing for a manual channel or given for the System channel.
    /// </exception>
    public Interaction(
        int id,
        int customerId,
        ContactChannel channel,
        string summary,
        DateTime occurredAt,
        int? employeeId,
        int? ticketId)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            throw new ValidationException("Nội dung tương tác không được để trống.");
        }

        bool isSystem = channel == ContactChannel.System;
        if (isSystem != (employeeId is null))
        {
            throw new ValidationException(isSystem
                ? "Tương tác do hệ thống tự ghi không gắn với nhân viên."
                : "Tương tác ghi tay phải có nhân viên ghi nhận.");
        }

        Id = id;
        CustomerId = customerId;
        Channel = channel;
        Summary = summary.Trim();
        OccurredAt = occurredAt;
        EmployeeId = employeeId;
        TicketId = ticketId;
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Customer the interaction is about.</summary>
    public int CustomerId { get; }

    /// <summary>How it took place.</summary>
    public ContactChannel Channel { get; }

    /// <summary>What happened; never empty.</summary>
    public string Summary { get; }

    /// <summary>When it happened.</summary>
    public DateTime OccurredAt { get; }

    /// <summary>Employee who recorded it, or null when the system recorded it.</summary>
    public int? EmployeeId { get; }

    /// <summary>Ticket the interaction comes from, or null.</summary>
    public int? TicketId { get; }
}
