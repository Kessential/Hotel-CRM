namespace HotelCRM.Domain.Customers;

/// <summary>
/// One entry of a customer's interaction history. Immutable. Created only through
/// <see cref="Customer.LogInteraction"/>, so an interaction never exists outside a history (composition).
/// </summary>
public sealed class Interaction
{
    /// <summary>Creates an entry; the summary has already been validated by <see cref="InteractionHistory"/>.</summary>
    internal Interaction(ContactChannel channel, string summary, DateTime occurredAt, int? ticketId)
    {
        Channel = channel;
        Summary = summary;
        OccurredAt = occurredAt;
        TicketId = ticketId;
    }

    /// <summary>When the interaction happened.</summary>
    public DateTime OccurredAt { get; }

    /// <summary>How it happened.</summary>
    public ContactChannel Channel { get; }

    /// <summary>What happened; never empty.</summary>
    public string Summary { get; }

    /// <summary>Id of the ticket the interaction is about, or null when it is not about a ticket.</summary>
    public int? TicketId { get; }

    /// <summary>Rebuilds an entry from stored data.</summary>
    internal static Interaction Restore(ContactChannel channel, string summary, DateTime occurredAt, int? ticketId)
    {
        return new Interaction(channel, summary, occurredAt, ticketId);
    }
}
