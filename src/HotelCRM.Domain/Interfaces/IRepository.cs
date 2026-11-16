namespace HotelCRM.Domain.Interfaces;

/// <summary>
/// Storage of one entity type (Repository pattern). Services depend on this interface only, so the
/// JSON files can be replaced by an in-memory list in tests without changing any service.
/// A repository loads its data once and always returns the same object for the same Id.
/// </summary>
/// <typeparam name="T">Entity type kept by the repository.</typeparam>
public interface IRepository<T>
    where T : IEntity
{
    /// <summary>Returns every stored entity, in the order they were added.</summary>
    IReadOnlyList<T> GetAll();

    /// <summary>Returns the entity with the given Id, or null when there is none.</summary>
    T? GetById(int id);

    /// <summary>
    /// Returns the Id to use for the next new entity: the largest stored Id plus 1 (1 when empty).
    /// The service asks for the Id first and then creates the entity.
    /// </summary>
    int NextId();

    /// <summary>
    /// Stores the entity: adds it when its Id is new, otherwise keeps the existing entry.
    /// In both cases the whole collection is persisted, so call this after every change.
    /// </summary>
    void Save(T entity);

    /// <summary>Removes the entity with the given Id and persists the collection. Does nothing when the Id is unknown.</summary>
    void Delete(int id);
}
