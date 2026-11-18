using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// The list of interactions of one customer. Owned by the <see cref="Customer"/> (composition): it is
/// created with the customer, stored inside the customer's record and cannot be shared.
/// </summary>
public sealed class InteractionHistory
{
    private readonly List<Interaction> _items = new();

    /// <summary>Creates an empty history; only <see cref="Customer"/> does this.</summary>
    internal InteractionHistory()
    {
    }

    /// <summary>Every interaction, oldest first.</summary>
    public IReadOnlyList<Interaction> Items => _items.AsReadOnly();

    /// <summary>Number of interactions.</summary>
    public int Count => _items.Count;

    /// <summary>Rebuilds a history from stored entries, keeping their order.</summary>
    internal static InteractionHistory Restore(IEnumerable<Interaction> items)
    {
        InteractionHistory history = new();
        history._items.AddRange(items);
        return history;
    }

    /// <summary>Creates a new interaction, appends it and returns it.</summary>
    /// <exception cref="ValidationException">The summary is empty.</exception>
    internal Interaction Add(ContactChannel channel, string summary, DateTime occurredAt, int? ticketId)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            throw new ValidationException("Interaction summary is required.");
        }

        Interaction interaction = new(channel, summary.Trim(), occurredAt, ticketId);
        _items.Add(interaction);
        return interaction;
    }
}
