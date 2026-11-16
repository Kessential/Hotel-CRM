using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Tickets;

/// <summary>
/// A complaint about the stay. Handled by Guest Relations within a base time of 24 hours;
/// closing it gives the customer 50 goodwill points instead of 10.
/// </summary>
public sealed class ComplaintTicket : SupportTicket
{
    /// <summary>Value of <see cref="TypeName"/> for this class.</summary>
    public const string TypeKey = "Complaint";

    /// <summary>Creates a new complaint; see the base constructor for the creation rules.</summary>
    /// <exception cref="ValidationException">The title is empty.</exception>
    public ComplaintTicket(
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

    private ComplaintTicket(TicketSnapshot snapshot)
        : base(snapshot)
    {
    }

    /// <inheritdoc />
    public override string TypeName => TypeKey;

    /// <summary>24 hours.</summary>
    public override int BaseSlaHours => throw new NotImplementedException();

    /// <summary><c>GUEST</c> (Guest Relations).</summary>
    public override string HandlingDepartmentCode => throw new NotImplementedException();

    /// <summary>50 points.</summary>
    public override int GoodwillPoints => throw new NotImplementedException();

    /// <summary>A complaint must describe what happened: the description is required.</summary>
    /// <param name="roomInActiveStay">Not used by this kind of ticket.</param>
    /// <exception cref="ValidationException">The description is empty.</exception>
    public override void Validate(bool roomInActiveStay)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rebuilds a complaint from stored data.</summary>
    internal static ComplaintTicket Restore(TicketSnapshot snapshot)
    {
        return new ComplaintTicket(snapshot);
    }
}
