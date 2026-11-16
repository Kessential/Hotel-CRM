namespace HotelCRM.Domain.Interfaces;

/// <summary>An object that is stored in a repository and identified by an integer.</summary>
public interface IEntity
{
    /// <summary>Identifier, unique within the entity type; obtained from <see cref="IRepository{T}.NextId"/>.</summary>
    int Id { get; }
}
