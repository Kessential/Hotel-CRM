using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Domain.Staff;

/// <summary>
/// A hotel department. Aggregates the agents who work in it: the agents exist on their own and
/// register themselves here when they are created.
/// </summary>
public sealed class Department : IEntity
{
    private readonly List<Agent> _agents = new();

    /// <summary>Creates a department.</summary>
    /// <exception cref="ValidationException">Code or name is empty.</exception>
    public Department(int id, string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ValidationException("Department code is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Department name is required.");
        }

        Id = id;
        Code = code.Trim();
        Name = name.Trim();
    }

    /// <summary>Identifier assigned by the repository.</summary>
    public int Id { get; }

    /// <summary>
    /// Short code used to route tickets: <c>GUEST</c> (Guest Relations), <c>HOUSE</c> (Housekeeping),
    /// <c>ENG</c> (Engineering), <c>FRONT</c> (Front Office).
    /// </summary>
    public string Code { get; }

    /// <summary>Display name.</summary>
    public string Name { get; }

    /// <summary>Agents who belong to this department.</summary>
    public IReadOnlyList<Agent> Agents => _agents.AsReadOnly();

    /// <summary>Returns the display name, so the department can be shown directly in lists.</summary>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>Rebuilds a department from stored data.</summary>
    internal static Department Restore(int id, string code, string name)
    {
        return new Department(id, code, name);
    }

    /// <summary>Called by <see cref="Agent"/> when an agent of this department is created.</summary>
    internal void AddAgent(Agent agent)
    {
        if (!_agents.Contains(agent))
        {
            _agents.Add(agent);
        }
    }
}
