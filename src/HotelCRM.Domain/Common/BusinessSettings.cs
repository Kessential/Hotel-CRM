namespace HotelCRM.Domain.Common;

/// <summary>
/// Limits read from <c>appsettings.json</c> and handed to the services. Domain classes never read
/// configuration themselves: the values reach them as method parameters.
/// </summary>
public sealed class BusinessSettings
{
    /// <summary>Creates the settings and checks that both limits are positive.</summary>
    /// <exception cref="ValidationException">A limit is zero or negative.</exception>
    public BusinessSettings(int maxActiveTickets, int reopenDays)
    {
        if (maxActiveTickets <= 0)
        {
            throw new ValidationException("MaxActiveTickets must be greater than 0.");
        }

        if (reopenDays <= 0)
        {
            throw new ValidationException("ReopenDays must be greater than 0.");
        }

        MaxActiveTickets = maxActiveTickets;
        ReopenDays = reopenDays;
    }

    /// <summary>Rule 3: largest number of active tickets one agent may hold. Default 5.</summary>
    public int MaxActiveTickets { get; }

    /// <summary>Rule 2: number of days after resolution during which a ticket can be reopened. Default 7.</summary>
    public int ReopenDays { get; }
}
