using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Tickets;

/// <summary>
/// A request for a service such as extra towels or a late check-out. Handled by Housekeeping within
/// a base time of 8 hours. Uses the default goodwill points and the default validation.
/// </summary>
public sealed class ServiceRequestTicket : SupportTicket
{
    /// <summary>Value of <see cref="TypeName"/> for this class.</summary>
    public const string TypeKey = "ServiceRequest";

    /// <summary>Creates a new service request; see the base constructor for the creation rules.</summary>
    /// <exception cref="ValidationException">The title is empty.</exception>
    public ServiceRequestTicket(
        int id,
        int customerId,
        string title,
        string description,
        TicketPriority requestedPriority,
        CustomerSlaProfile slaProfile,
        DateTime createdAt)
        : base(id, customerId, title, description, requestedPriority, slaProfile, createdAt)
    {
    }

    private ServiceRequestTicket(TicketSnapshot snapshot)
        : base(snapshot)
    {
    }

    /// <inheritdoc />
    public override string TypeName => TypeKey;

    /// <summary>8 hours.</summary>
    public override int BaseSlaHours => 8;

    /// <summary><c>HOUSE</c> (Housekeeping).</summary>
    public override string HandlingDepartmentCode => "HOUSE";

    /// <summary>Rebuilds a service request from stored data.</summary>
    internal static ServiceRequestTicket Restore(TicketSnapshot snapshot)
    {
        return new ServiceRequestTicket(snapshot);
    }
}
