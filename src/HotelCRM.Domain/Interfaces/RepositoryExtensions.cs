using HotelCRM.Domain.Common;

namespace HotelCRM.Domain.Interfaces;

/// <summary>Helpers shared by every <see cref="IRepository{T}"/>.</summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Returns the entity with the given Id and fails when it does not exist. Services use this
    /// instead of throwing themselves, so that every business error still comes from the domain layer.
    /// </summary>
    /// <exception cref="BusinessRuleException">No entity has this Id.</exception>
    public static T GetRequired<T>(this IRepository<T> repository, int id)
        where T : IEntity
    {
        T? entity = repository.GetById(id);
        if (entity is null)
        {
            throw new BusinessRuleException($"{typeof(T).Name} {id} does not exist.");
        }

        return entity;
    }
}
