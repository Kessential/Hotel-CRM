namespace HotelCRM.Domain.Common;

/// <summary>
/// Base class of every error raised by the domain layer. The user interface shows the message
/// of a <see cref="DomainException"/> as a warning; any other exception is logged as unexpected.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>Creates the exception with a message that can be shown to the user.</summary>
    protected DomainException(string message)
        : base(message)
    {
    }
}
