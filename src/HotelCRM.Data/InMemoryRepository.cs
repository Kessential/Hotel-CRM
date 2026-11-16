using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Data;

/// <summary>
/// Repository that keeps its entities in a list and never touches a file. Used by the unit tests in
/// place of <see cref="JsonRepository{TEntity, TRecord}"/>; the services cannot tell the difference
/// because they only know <see cref="IRepository{T}"/>.
/// </summary>
/// <typeparam name="T">Entity type kept by the repository.</typeparam>
public sealed class InMemoryRepository<T> : IRepository<T>
    where T : IEntity
{
    private readonly List<T> _items = new();

    /// <inheritdoc />
    public IReadOnlyList<T> GetAll()
    {
        return _items.ToArray();
    }

    /// <inheritdoc />
    public T? GetById(int id)
    {
        return _items.FirstOrDefault(item => item.Id == id);
    }

    /// <inheritdoc />
    public int NextId()
    {
        return _items.Count == 0 ? 1 : _items.Max(item => item.Id) + 1;
    }

    /// <inheritdoc />
    public void Save(T entity)
    {
        int index = _items.FindIndex(item => item.Id == entity.Id);
        if (index < 0)
        {
            _items.Add(entity);
        }
        else
        {
            _items[index] = entity;
        }
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        _items.RemoveAll(item => item.Id == id);
    }
}
