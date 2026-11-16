using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Common;

/// <summary>
/// Base class of every person known to the hotel: customers and employees.
/// Guarantees that the name is present and that phone and email are well formed (rule 6, format part).
/// </summary>
public abstract class Person : IEntity
{
    private const int PhoneLength = 10;

    /// <summary>Creates a person and validates the name, phone and email.</summary>
    /// <exception cref="ValidationException">
    /// The name is empty, the phone is not exactly 10 digits starting with 0, or the email is malformed.
    /// </exception>
    protected Person(int id, string fullName, string phone, string email)
    {
        Id = id;
        FullName = ValidateFullName(fullName);
        Phone = ValidatePhone(phone);
        Email = ValidateEmail(email);
    }

    /// <summary>Identifier, unique within the entity type; assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>Full name; never empty.</summary>
    public string FullName { get; private set; }

    /// <summary>Phone number: exactly 10 digits, starting with 0.</summary>
    public string Phone { get; private set; }

    /// <summary>Email address: exactly one '@', a dot after it and no whitespace.</summary>
    public string Email { get; private set; }

    /// <summary>Checks the phone format: exactly 10 digits, the first one being 0.</summary>
    public static bool IsValidPhone(string? phone)
    {
        return phone is not null
            && phone.Length == PhoneLength
            && phone[0] == '0'
            && phone.All(c => c is >= '0' and <= '9');
    }

    /// <summary>
    /// Checks the email format: no whitespace, exactly one '@' that is not the first character,
    /// and a dot somewhere after the '@' that is neither right after it nor the last character.
    /// </summary>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrEmpty(email) || email.Any(char.IsWhiteSpace))
        {
            return false;
        }

        int at = email.IndexOf('@');
        if (at <= 0 || at != email.LastIndexOf('@'))
        {
            return false;
        }

        int dot = email.IndexOf('.', at + 1);
        return dot > at + 1 && !email.EndsWith('.');
    }

    /// <summary>Replaces name, phone and email after validating all three; nothing changes if one is invalid.</summary>
    /// <exception cref="ValidationException">Same conditions as the constructor.</exception>
    protected void ChangeContact(string fullName, string phone, string email)
    {
        string newFullName = ValidateFullName(fullName);
        string newPhone = ValidatePhone(phone);
        string newEmail = ValidateEmail(email);

        FullName = newFullName;
        Phone = newPhone;
        Email = newEmail;
    }

    private static string ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ValidationException("Full name is required.");
        }

        return fullName.Trim();
    }

    private static string ValidatePhone(string phone)
    {
        string trimmed = (phone ?? string.Empty).Trim();
        if (!IsValidPhone(trimmed))
        {
            throw new ValidationException("Phone number must be exactly 10 digits and start with 0.");
        }

        return trimmed;
    }

    private static string ValidateEmail(string email)
    {
        string trimmed = (email ?? string.Empty).Trim();
        if (!IsValidEmail(trimmed))
        {
            throw new ValidationException("Email address is not valid.");
        }

        return trimmed;
    }
}
