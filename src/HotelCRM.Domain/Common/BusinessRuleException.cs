namespace HotelCRM.Domain.Common;

/// <summary>
/// A rule that involves several objects, the current state of an object or the permissions of the
/// acting employee was violated.
/// </summary>
public sealed class BusinessRuleException : DomainException
{
    /// <summary>Creates the exception with a message that can be shown to the user.</summary>
    public BusinessRuleException(string message)
        : base(message)
    {
    }
}
