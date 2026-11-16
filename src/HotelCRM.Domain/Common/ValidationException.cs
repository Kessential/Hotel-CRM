namespace HotelCRM.Domain.Common;

/// <summary>
/// The data of a single object is invalid: wrong format, missing required field or value out of range.
/// </summary>
public sealed class ValidationException : DomainException
{
    /// <summary>Creates the exception with a message that can be shown to the user.</summary>
    public ValidationException(string message)
        : base(message)
    {
    }
}
