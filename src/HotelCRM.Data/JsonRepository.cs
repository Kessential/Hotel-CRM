using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HotelCRM.Domain.Common;
using HotelCRM.Domain.Interfaces;

namespace HotelCRM.Data;

/// <summary>
/// Repository backed by one JSON file. The file is read once, in the constructor; afterwards every
/// read is served from memory, so the same Id always gives the same object. Every
/// <see cref="Save"/> and <see cref="Delete"/> writes the whole file again.
/// </summary>
/// <typeparam name="TEntity">Domain entity type.</typeparam>
/// <typeparam name="TRecord">Flat record type stored in the file.</typeparam>
public sealed class JsonRepository<TEntity, TRecord> : IRepository<TEntity>
    where TEntity : IEntity
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly string _filePath;
    private readonly IMapper<TEntity, TRecord> _mapper;
    private readonly List<TEntity> _items;

    /// <summary>Creates the repository and loads the file.</summary>
    /// <param name="filePath">Full path of the JSON file; it must contain an array of records.</param>
    /// <param name="mapper">Converter between entity and record.</param>
    /// <exception cref="FileNotFoundException">The file does not exist.</exception>
    /// <exception cref="InvalidDataException">The file is not valid JSON, or a record cannot be turned into an entity.</exception>
    public JsonRepository(string filePath, IMapper<TEntity, TRecord> mapper)
    {
        _filePath = filePath;
        _mapper = mapper;
        _items = Load();
    }

    /// <inheritdoc />
    public IReadOnlyList<TEntity> GetAll()
    {
        return _items.ToArray();
    }

    /// <inheritdoc />
    public TEntity? GetById(int id)
    {
        return _items.FirstOrDefault(item => item.Id == id);
    }

    /// <inheritdoc />
    public int NextId()
    {
        return _items.Count == 0 ? 1 : _items.Max(item => item.Id) + 1;
    }

    /// <inheritdoc />
    /// <exception cref="IOException">The file cannot be written.</exception>
    public void Save(TEntity entity)
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

        Persist();
    }

    /// <inheritdoc />
    /// <exception cref="IOException">The file cannot be written.</exception>
    public void Delete(int id)
    {
        if (_items.RemoveAll(item => item.Id == id) > 0)
        {
            Persist();
        }
    }

    private List<TEntity> Load()
    {
        string fileName = Path.GetFileName(_filePath);
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"Data file '{fileName}' was not found.", _filePath);
        }

        try
        {
            List<TRecord> records = JsonSerializer.Deserialize<List<TRecord>>(File.ReadAllText(_filePath), _jsonOptions)
                ?? new List<TRecord>();
            return records.Select(_mapper.ToEntity).ToList();
        }
        catch (Exception ex) when (ex is JsonException or InvalidDataException or ArgumentException or DomainException)
        {
            throw new InvalidDataException($"Data file '{fileName}' could not be loaded: {ex.Message}", ex);
        }
    }

    /// <summary>Writes every entity to a temporary file and then replaces the data file, so a failed write cannot leave half a file.</summary>
    private void Persist()
    {
        List<TRecord> records = _items.Select(_mapper.ToRecord).ToList();
        string json = JsonSerializer.Serialize(records, _jsonOptions);
        string tempPath = _filePath + ".tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }
}
