using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// A hotel guest. Owns the loyalty account (points and membership tier) and the interaction history.
/// Fields entered in the customer form (UC6): full name, phone, email, identity number, nationality
/// and notes; the first five are required.
/// </summary>
public sealed class Customer : Person
{
    /// <summary>Creates a new customer in the Standard tier with 0 points and an empty history.</summary>
    /// <param name="id">Id obtained from the customer repository.</param>
    /// <param name="fullName">Full name; required.</param>
    /// <param name="phone">Phone: exactly 10 digits starting with 0.</param>
    /// <param name="email">Email: one '@', a dot after it, no whitespace.</param>
    /// <param name="identityNumber">Passport or national identity number; required.</param>
    /// <param name="nationality">Nationality; required.</param>
    /// <param name="notes">Free-text notes such as preferences; may be empty.</param>
    /// <exception cref="ValidationException">
    /// Name, phone or email is invalid (rule 6, format), or identity number or nationality is empty.
    /// </exception>
    public Customer(
        int id,
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes)
        : this(id, fullName, phone, email, identityNumber, nationality, notes, new StandardTier(), 0, new InteractionHistory())
    {
        ValidateProfile();
    }

    private Customer(
        int id,
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes,
        MembershipTier tier,
        int points,
        InteractionHistory history)
        : base(id, fullName, phone, email)
    {
        IdentityNumber = identityNumber;
        Nationality = nationality;
        Notes = notes;
        Tier = tier;
        Points = points;
        History = history;
    }

    /// <summary>Passport or national identity number; never empty.</summary>
    public string IdentityNumber { get; private set; }

    /// <summary>Nationality; never empty.</summary>
    public string Nationality { get; private set; }

    /// <summary>Free-text notes; may be empty.</summary>
    public string Notes { get; private set; }

    /// <summary>Current membership tier. Only goes up (rule 8).</summary>
    public MembershipTier Tier { get; private set; }

    /// <summary>Loyalty points collected so far. Only goes up (rule 8).</summary>
    public int Points { get; private set; }

    /// <summary>Interaction history owned by this customer.</summary>
    public InteractionHistory History { get; }

    /// <summary>Replaces the editable fields; nothing changes if one of them is invalid.</summary>
    /// <exception cref="ValidationException">Same conditions as the constructor.</exception>
    public void UpdateProfile(
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes)
    {
        string newIdentityNumber = ValidateIdentityNumber(identityNumber);
        string newNationality = ValidateNationality(nationality);
        ChangeContact(fullName, phone, email);

        IdentityNumber = newIdentityNumber;
        Nationality = newNationality;
        Notes = NormalizeNotes(notes);
    }

    /// <summary>Rule 6: stops a registration or update that would duplicate another customer's phone or email.</summary>
    /// <param name="phoneTaken">Whether another customer already has this phone number; looked up by the service.</param>
    /// <param name="emailTaken">Whether another customer already has this email (compared case-insensitively); looked up by the service.</param>
    /// <exception cref="BusinessRuleException">Either flag is true.</exception>
    public void EnsureUnique(bool phoneTaken, bool emailTaken)
    {
        if (phoneTaken && emailTaken)
        {
            throw new BusinessRuleException(
                $"Another customer already uses phone number {Phone} and email address {Email}.");
        }

        if (phoneTaken)
        {
            throw new BusinessRuleException($"Another customer already uses phone number {Phone}.");
        }

        if (emailTaken)
        {
            throw new BusinessRuleException($"Another customer already uses email address {Email}.");
        }
    }

    /// <summary>Rule 10: stops the deletion of a customer who still has tickets that are not closed.</summary>
    /// <param name="hasOpenTickets">Result of <c>ITicketQuery.HasOpenTickets</c> for this customer.</param>
    /// <exception cref="BusinessRuleException"><paramref name="hasOpenTickets"/> is true.</exception>
    public void EnsureCanBeDeleted(bool hasOpenTickets)
    {
        if (hasOpenTickets)
        {
            throw new BusinessRuleException(
                $"Customer {FullName} still has tickets that are not closed and cannot be deleted.");
        }
    }

    /// <summary>
    /// Rule 8: adds loyalty points, then moves up to the highest tier whose
    /// <see cref="MembershipTier.MinPoints"/> has been reached. Several tiers can be passed in one call;
    /// the tier never goes down.
    /// </summary>
    /// <param name="amount">Points to add; must be greater than 0.</param>
    /// <exception cref="ValidationException"><paramref name="amount"/> is zero or negative.</exception>
    public void AddPoints(int amount)
    {
        if (amount <= 0)
        {
            throw new ValidationException("Points to add must be greater than 0.");
        }

        Points += amount;
        while (Tier.Next() is { } next && Points >= next.MinPoints)
        {
            Tier = next;
        }
    }

    /// <summary>Appends an entry to <see cref="History"/> and returns it.</summary>
    /// <param name="channel">How the interaction took place.</param>
    /// <param name="summary">What happened; required.</param>
    /// <param name="occurredAt">When it happened (the caller's "now").</param>
    /// <param name="ticketId">Id of the related ticket, or null.</param>
    /// <exception cref="ValidationException">The summary is empty.</exception>
    public Interaction LogInteraction(ContactChannel channel, string summary, DateTime occurredAt, int? ticketId)
    {
        return History.Add(channel, summary, occurredAt, ticketId);
    }

    /// <summary>Rebuilds a customer from stored data, including tier, points and history.</summary>
    internal static Customer Restore(
        int id,
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        string notes,
        MembershipTier tier,
        int points,
        InteractionHistory history)
    {
        return new Customer(id, fullName, phone, email, identityNumber, nationality, notes, tier, points, history);
    }

    /// <summary>Throws <see cref="ValidationException"/> when identity number or nationality is empty.</summary>
    private void ValidateProfile()
    {
        IdentityNumber = ValidateIdentityNumber(IdentityNumber);
        Nationality = ValidateNationality(Nationality);
        Notes = NormalizeNotes(Notes);
    }

    private static string ValidateIdentityNumber(string identityNumber)
    {
        if (string.IsNullOrWhiteSpace(identityNumber))
        {
            throw new ValidationException("Identity number is required.");
        }

        return identityNumber.Trim();
    }

    private static string ValidateNationality(string nationality)
    {
        if (string.IsNullOrWhiteSpace(nationality))
        {
            throw new ValidationException("Nationality is required.");
        }

        return nationality.Trim();
    }

    private static string NormalizeNotes(string notes)
    {
        return (notes ?? string.Empty).Trim();
    }
}
