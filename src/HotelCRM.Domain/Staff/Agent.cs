using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Staff;

/// <summary>
/// An employee who handles tickets. An agent may start and resolve the tickets assigned to them,
/// but may not assign, close or reopen tickets (rule 11).
/// </summary>
public sealed class Agent : Employee
{
    /// <summary>Creates an agent and registers them in their department.</summary>
    /// <exception cref="ValidationException">See <see cref="Employee"/>.</exception>
    public Agent(
        int id,
        string fullName,
        string phone,
        string email,
        string username,
        string passwordHash,
        string salt,
        Department department)
        : base(id, fullName, phone, email, username, passwordHash, salt, department)
    {
        department.AddAgent(this);
    }

    /// <inheritdoc />
    public override EmployeeRole Role => EmployeeRole.Agent;

    /// <inheritdoc />
    public override bool CanAssign()
    {
        return false;
    }

    /// <inheritdoc />
    public override bool CanClose()
    {
        return false;
    }

    /// <inheritdoc />
    public override bool CanReopen()
    {
        return false;
    }

    /// <summary>
    /// Rule 3: whether this agent can take one more ticket, i.e. <paramref name="activeCount"/> is
    /// below <paramref name="maxActive"/>. Used to build the candidate list for automatic assignment.
    /// </summary>
    /// <param name="activeCount">Tickets assigned to this agent that are Open or InProgress.</param>
    /// <param name="maxActive"><see cref="BusinessSettings.MaxActiveTickets"/>.</param>
    public bool CanTake(int activeCount, int maxActive)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rule 3: stops an assignment that would exceed the limit of active tickets.</summary>
    /// <param name="activeCount">Tickets assigned to this agent that are Open or InProgress.</param>
    /// <param name="maxActive"><see cref="BusinessSettings.MaxActiveTickets"/>.</param>
    /// <exception cref="BusinessRuleException">
    /// <paramref name="activeCount"/> is greater than or equal to <paramref name="maxActive"/>.
    /// </exception>
    public void EnsureCanTake(int activeCount, int maxActive)
    {
        throw new NotImplementedException();
    }

    /// <summary>Rebuilds an agent from stored data.</summary>
    internal static Agent Restore(
        int id,
        string fullName,
        string phone,
        string email,
        string username,
        string passwordHash,
        string salt,
        Department department)
    {
        return new Agent(id, fullName, phone, email, username, passwordHash, salt, department);
    }
}
