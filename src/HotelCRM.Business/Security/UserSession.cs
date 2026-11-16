using HotelCRM.Domain.Common;

namespace HotelCRM.Business.Security;

/// <summary>
/// Remembers who is logged in. Forms read <see cref="Current"/> to show the right menus and pass it
/// as the <c>actor</c> of service calls. Services never read the session: the acting employee always
/// reaches them as a parameter. Only <c>AuthService</c> can start or end the session.
/// </summary>
public static class UserSession
{
    private static Employee? _current;

    /// <summary>Whether somebody is logged in.</summary>
    public static bool IsLoggedIn => _current is not null;

    /// <summary>The logged-in employee.</summary>
    /// <exception cref="InvalidOperationException">Nobody is logged in (a programming error, not a business error).</exception>
    public static Employee Current => _current ?? throw new InvalidOperationException("No user is logged in.");

    /// <summary>Starts the session of the given employee.</summary>
    internal static void Start(Employee employee)
    {
        _current = employee;
    }

    /// <summary>Ends the current session, if any.</summary>
    internal static void End()
    {
        _current = null;
    }
}
