using System.Text.Json;
using HotelCRM.Domain.Common;

namespace HotelCRM.Data;

/// <summary>
/// Application configuration read from <c>appsettings.json</c> next to the executable (Singleton
/// pattern: one instance, created on first use). Only <c>Program.cs</c> uses it; every other class
/// receives the values it needs through its constructor.
/// </summary>
public sealed class AppConfig
{
    private const string FileName = "appsettings.json";
    private const string DefaultDataFolder = "Data";
    private const string DefaultStrategy = "LeastLoaded";
    private const int DefaultMaxActiveTickets = 5;
    private const int DefaultReopenDays = 7;

    private static AppConfig? _instance;

    private AppConfig(string dataFolder, string defaultAssignmentStrategy, BusinessSettings rules)
    {
        DataFolder = dataFolder;
        DefaultAssignmentStrategy = defaultAssignmentStrategy;
        Rules = rules;
    }

    /// <summary>The single configuration object; the file is read the first time this is used.</summary>
    /// <exception cref="FileNotFoundException"><c>appsettings.json</c> is missing.</exception>
    /// <exception cref="InvalidDataException"><c>appsettings.json</c> is not valid JSON.</exception>
    /// <exception cref="ValidationException"><c>MaxActiveTickets</c> or <c>ReopenDays</c> is not greater than 0.</exception>
    public static AppConfig Instance => _instance ??= Load();

    /// <summary>
    /// Full path of the folder that holds the JSON data files. A relative <c>DataFolder</c> setting
    /// is resolved against the folder of the executable, never against a fixed drive.
    /// </summary>
    public string DataFolder { get; }

    /// <summary>Name of the assignment strategy to use: <c>LeastLoaded</c>, <c>DepartmentMatch</c> or <c>RoundRobin</c>.</summary>
    public string DefaultAssignmentStrategy { get; }

    /// <summary>Business limits (<c>MaxActiveTickets</c>, <c>ReopenDays</c>) handed to the services.</summary>
    public BusinessSettings Rules { get; }

    /// <summary>Returns the full path of a data file such as <c>tickets.json</c>.</summary>
    public string GetDataFilePath(string fileName)
    {
        return Path.Combine(DataFolder, fileName);
    }

    private static AppConfig Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, FileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Configuration file '{FileName}' was not found next to the application.", path);
        }

        SettingsFile settings;
        try
        {
            settings = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText(path)) ?? new SettingsFile();
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Configuration file '{FileName}' is not valid JSON: {ex.Message}", ex);
        }

        string dataFolder = string.IsNullOrWhiteSpace(settings.DataFolder) ? DefaultDataFolder : settings.DataFolder;
        string strategy = string.IsNullOrWhiteSpace(settings.DefaultAssignmentStrategy)
            ? DefaultStrategy
            : settings.DefaultAssignmentStrategy.Trim();
        BusinessSettings rules = new(
            settings.MaxActiveTickets ?? DefaultMaxActiveTickets,
            settings.ReopenDays ?? DefaultReopenDays);

        return new AppConfig(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, dataFolder)), strategy, rules);
    }

    /// <summary>Shape of <c>appsettings.json</c>; a missing key falls back to its default.</summary>
    private sealed class SettingsFile
    {
        public string? DataFolder { get; set; }

        public string? DefaultAssignmentStrategy { get; set; }

        public int? MaxActiveTickets { get; set; }

        public int? ReopenDays { get; set; }
    }
}
