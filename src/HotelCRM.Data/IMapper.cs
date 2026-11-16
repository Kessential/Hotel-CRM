namespace HotelCRM.Data;

/// <summary>
/// Converts between a domain entity and its flat record. Entities keep their state private and
/// validate themselves, so they are never serialized directly: the record is what is written to JSON.
/// </summary>
/// <typeparam name="TEntity">Domain entity type.</typeparam>
/// <typeparam name="TRecord">Record type stored in the JSON file.</typeparam>
public interface IMapper<TEntity, TRecord>
{
    /// <summary>Copies the current state of an entity into a record.</summary>
    TRecord ToRecord(TEntity entity);

    /// <summary>
    /// Rebuilds an entity from a record with the entity's <c>Restore</c> method, which does not run
    /// the business rules again.
    /// </summary>
    /// <exception cref="InvalidDataException">The record refers to something that does not exist.</exception>
    TEntity ToEntity(TRecord record);
}
