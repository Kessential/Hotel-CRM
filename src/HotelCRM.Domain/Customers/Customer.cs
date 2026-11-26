using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Customers;

/// <summary>
/// A hotel guest (BM4, UC07). Decides Q7 (valid identity document, date of birth, no duplicates),
/// Q9 (lifetime points and tier upgrade) and Q13 (when a customer may be deleted).
/// Stays, interactions and point transactions are separate entities that refer to the customer by Id,
/// so the customer does not hold them in lists; repositories read them when needed.
/// </summary>
public sealed class Customer : Person
{
    /// <summary>Q7: the nationality whose customers identify with a citizen ID card instead of a passport.</summary>
    public const string VietnameseNationality = "Việt Nam";

    private const int CitizenIdLength = 12;
    private const int PassportMinLength = 6;
    private const int PassportMaxLength = 9;

    /// <summary>Registers a new customer: Standard tier, 0 lifetime points, created at <paramref name="now"/>.</summary>
    /// <param name="id">Id assigned by the customer repository.</param>
    /// <param name="fullName">Full name; required.</param>
    /// <param name="phone">Exactly 10 digits starting with 0 (checked by <see cref="Person"/>).</param>
    /// <param name="email">One '@', a dot after it, no whitespace (checked by <see cref="Person"/>).</param>
    /// <param name="identityNumber">
    /// Citizen ID card (12 digits) when <paramref name="nationality"/> is <see cref="VietnameseNationality"/>,
    /// otherwise a passport number (6 to 9 letters A–Z and digits, stored in upper case).
    /// </param>
    /// <param name="nationality">Required.</param>
    /// <param name="dateOfBirth">Optional; only the date part is kept. May not be after <paramref name="now"/>.</param>
    /// <param name="marketingConsent">Whether the customer agrees to receive offers (used by campaigns, Q11).</param>
    /// <param name="notes">Free text such as preferences; may be empty.</param>
    /// <param name="now">Current time, from <c>IClock</c>.</param>
    /// <exception cref="ValidationException">A field is missing or has the wrong format (Q7).</exception>
    public Customer(
        int id,
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        DateTime? dateOfBirth,
        bool marketingConsent,
        string notes,
        DateTime now)
        : base(id, fullName, phone, email)
    {
        (Nationality, IdentityNumber, DateOfBirth) = ValidateProfile(identityNumber, nationality, dateOfBirth, now);
        MarketingConsent = marketingConsent;
        Notes = NormalizeNotes(notes);
        Tier = new StandardTier();
        LifetimePoints = 0;
        CreatedAt = now;
    }

    /// <summary>Citizen ID card or passport number, depending on <see cref="Nationality"/>.</summary>
    public string IdentityNumber { get; private set; }

    /// <summary>Nationality; never empty.</summary>
    public string Nationality { get; private set; }

    /// <summary>Date of birth (date only), or null when not given.</summary>
    public DateTime? DateOfBirth { get; private set; }

    /// <summary>Whether the customer agrees to receive offers; campaigns only target customers who agree (Q11).</summary>
    public bool MarketingConsent { get; private set; }

    /// <summary>Free-text notes; never null, may be empty.</summary>
    public string Notes { get; private set; }

    /// <summary>Current membership tier. Only <see cref="EarnPoints"/> changes it, and only upwards (Q9).</summary>
    public MembershipTier Tier { get; private set; }

    /// <summary>
    /// Q9: points earned from stays and goodwill. Points spent on rewards are not subtracted, so the tier
    /// never goes down. Not a database column: the repository sums the ledger when it loads the customer.
    /// </summary>
    public int LifetimePoints { get; private set; }

    /// <summary>When the customer was registered.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Replaces the editable fields. Every field is checked before any is assigned, so a failed update
    /// leaves the customer unchanged.
    /// </summary>
    /// <exception cref="ValidationException">Same conditions as the constructor.</exception>
    public void UpdateProfile(
        string fullName,
        string phone,
        string email,
        string identityNumber,
        string nationality,
        DateTime? dateOfBirth,
        bool marketingConsent,
        string notes,
        DateTime now)
    {
        var (newNationality, newIdentityNumber, newDateOfBirth) =
            ValidateProfile(identityNumber, nationality, dateOfBirth, now);
        ChangeContact(fullName, phone, email);

        Nationality = newNationality;
        IdentityNumber = newIdentityNumber;
        DateOfBirth = newDateOfBirth;
        MarketingConsent = marketingConsent;
        Notes = NormalizeNotes(notes);
    }

    /// <summary>
    /// Q7: stops a registration or update that would reuse another customer's phone, email or identity number.
    /// The service looks the three flags up, leaving this customer out when it is an update.
    /// </summary>
    /// <param name="phoneTaken">Another customer already has this phone number.</param>
    /// <param name="emailTaken">Another customer already has this email (case-insensitive).</param>
    /// <param name="identityTaken">Another customer already has this identity number.</param>
    /// <exception cref="BusinessRuleException">At least one flag is true; the message names every duplicated field.</exception>
    public void EnsureUnique(bool phoneTaken, bool emailTaken, bool identityTaken)
    {
        List<string> taken = new();
        if (phoneTaken)
        {
            taken.Add("số điện thoại");
        }

        if (emailTaken)
        {
            taken.Add("email");
        }

        if (identityTaken)
        {
            taken.Add("số giấy tờ");
        }

        if (taken.Count > 0)
        {
            throw new BusinessRuleException($"Đã có khách khác dùng {string.Join(", ", taken)} này.");
        }
    }

    /// <summary>
    /// Q13: a customer with any stay (even a cancelled booking), any ticket (even a closed one) or any
    /// campaign target cannot be deleted, because those rows keep the CRM history and report R4.
    /// </summary>
    /// <param name="hasStays">The customer has at least one booking, in any status.</param>
    /// <param name="hasTickets">The customer has at least one ticket, from <c>ITicketQuery</c> (L5).</param>
    /// <param name="inCampaigns">The customer is a target of at least one campaign.</param>
    /// <exception cref="BusinessRuleException">At least one flag is true; the message names every reason.</exception>
    public void EnsureCanBeDeleted(bool hasStays, bool hasTickets, bool inCampaigns)
    {
        List<string> reasons = new();
        if (hasStays)
        {
            reasons.Add("đã có lần đặt phòng");
        }

        if (hasTickets)
        {
            reasons.Add("đã có ticket");
        }

        if (inCampaigns)
        {
            reasons.Add("đã nằm trong chiến dịch chăm sóc");
        }

        if (reasons.Count > 0)
        {
            throw new BusinessRuleException(
                $"Không xóa được khách {FullName} vì {string.Join(", ", reasons)}.");
        }
    }

    /// <summary>
    /// Q9: adds points from a stay or goodwill to <see cref="LifetimePoints"/>, then moves up to the highest
    /// tier whose <see cref="MembershipTier.MinPoints"/> is reached. One call can pass several tiers;
    /// the tier never goes down.
    /// </summary>
    /// <param name="points">Points earned; must be greater than 0 (a zero result is not recorded at all).</param>
    /// <returns>True when the tier changed, so the caller can tell the user.</returns>
    /// <exception cref="ValidationException"><paramref name="points"/> is zero or negative.</exception>
    public bool EarnPoints(int points)
    {
        if (points <= 0)
        {
            throw new ValidationException("Số điểm cộng phải lớn hơn 0.");
        }

        LifetimePoints += points;

        MembershipTier before = Tier;
        while (Tier.Next() is { } next && LifetimePoints >= next.MinPoints)
        {
            Tier = next;
        }

        return Tier != before;
    }

    /// <summary>
    /// Q7: checks nationality, identity document and date of birth, and returns the cleaned values.
    /// Returns instead of assigning so that <see cref="UpdateProfile"/> can check everything first.
    /// </summary>
    private static (string Nationality, string IdentityNumber, DateTime? DateOfBirth) ValidateProfile(
        string identityNumber,
        string nationality,
        DateTime? dateOfBirth,
        DateTime now)
    {
        if (string.IsNullOrWhiteSpace(nationality))
        {
            throw new ValidationException("Quốc tịch không được để trống.");
        }

        string cleanNationality = nationality.Trim();
        string cleanIdentity = (identityNumber ?? string.Empty).Trim().ToUpperInvariant();

        if (string.Equals(cleanNationality, VietnameseNationality, StringComparison.OrdinalIgnoreCase))
        {
            if (cleanIdentity.Length != CitizenIdLength || !cleanIdentity.All(char.IsAsciiDigit))
            {
                throw new ValidationException($"Số CCCD phải gồm đúng {CitizenIdLength} chữ số.");
            }
        }
        else if (cleanIdentity.Length is < PassportMinLength or > PassportMaxLength
            || !cleanIdentity.All(c => char.IsAsciiLetterUpper(c) || char.IsAsciiDigit(c)))
        {
            throw new ValidationException(
                $"Số hộ chiếu phải gồm {PassportMinLength}–{PassportMaxLength} ký tự chữ (A–Z) và số.");
        }

        DateTime? cleanDateOfBirth = dateOfBirth?.Date;
        if (cleanDateOfBirth > now.Date)
        {
            throw new ValidationException("Ngày sinh không được sau ngày hiện tại.");
        }

        return (cleanNationality, cleanIdentity, cleanDateOfBirth);
    }

    private static string NormalizeNotes(string notes)
    {
        return (notes ?? string.Empty).Trim();
    }
}
