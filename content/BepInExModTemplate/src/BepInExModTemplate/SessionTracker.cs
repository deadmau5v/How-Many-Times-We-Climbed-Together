using System.Collections.Generic;
using System.IO;
using BepInEx;
using Newtonsoft.Json;

namespace HowManyTimesWeClimbedTogether;

internal class SessionTracker
{
    private readonly string _dataFilePath;
    private Dictionary<string, PlayerRecord> _records;

    internal SessionTracker()
    {
        _dataFilePath = Path.Combine(Paths.ConfigPath, "HowManyTimesWeClimbedTogether.json");
        _records = Load();
    }

    internal void IncrementSession(string playerId, string playerName)
    {
        if (_records.TryGetValue(playerId, out var record))
        {
            record.SessionCount++;
            record.LastKnownName = playerName;
        }
        else
        {
            _records[playerId] = new PlayerRecord
            {
                PlayerId = playerId,
                LastKnownName = playerName,
                SessionCount = 1,
            };
        }
    }

    internal int GetSessionCount(string playerId)
    {
        return _records.TryGetValue(playerId, out var record) ? record.SessionCount : 0;
    }

    internal string GetLastKnownName(string playerId)
    {
        return _records.TryGetValue(playerId, out var record) && !string.IsNullOrWhiteSpace(record.LastKnownName)
            ? record.LastKnownName
            : "";
    }

    internal IReadOnlyDictionary<string, PlayerRecord> GetAllRecords() => _records;

    internal void Save()
    {
        try
        {
            string json = JsonConvert.SerializeObject(_records, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }
        catch (System.Exception ex)
        {
            Plugin.Log.LogError($"Failed to save session data: {ex.Message}");
        }
    }

    private Dictionary<string, PlayerRecord> Load()
    {
        try
        {
            if (File.Exists(_dataFilePath))
            {
                string json = File.ReadAllText(_dataFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, PlayerRecord>>(json)
                    ?? new Dictionary<string, PlayerRecord>();
            }
        }
        catch (System.Exception ex)
        {
            Plugin.Log.LogError($"Failed to load session data: {ex.Message}");
        }

        return new Dictionary<string, PlayerRecord>();
    }
}

internal class PlayerRecord
{
    public string PlayerId { get; set; } = "";
    public string LastKnownName { get; set; } = "";
    public int SessionCount { get; set; }
}
