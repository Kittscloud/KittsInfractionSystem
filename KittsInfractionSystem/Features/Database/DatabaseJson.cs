using KittsInfractionSystem.Features.Models;
using LabApi.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace KittsInfractionSystem.Features.Database;

internal static class DatabaseJson
{
    private static List<InfractionData> _jsonCache = [];
    private static string JsonFilePath => Path.Combine(
        KittsInfractionSystem.Instance.GetConfigDirectory().FullName, "Infractions.json");

    public static void Init() =>
        LoadJsonCache();

    public static void Stop()
    {
        SaveJsonCache();
        _jsonCache.Clear();
    }

    private static void EnsureJsonFile()
    {
        string dir = KittsInfractionSystem.Instance.GetConfigDirectory().FullName;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(JsonFilePath))
            File.WriteAllText(JsonFilePath, "[]");
    }

    private static void LoadJsonCache()
    {
        try
        {
            EnsureJsonFile();

            List<InfractionData> jsonCache = JsonConvert.DeserializeObject<List<InfractionData>>(File.ReadAllText(JsonFilePath));
            if (jsonCache != null)
                _jsonCache = jsonCache;
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.LoadJsonCache", $"Error loading JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.LoadJsonCache", e.ToString());
        }
    }

    private static void SaveJsonCache()
    {
        try
        {
            File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(_jsonCache, Formatting.Indented));
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.SaveJsonCache", $"Error saving JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.SaveJsonCache", e.ToString());
        }
    }

    public static void AddInfraction(InfractionData infraction)
    {
        _jsonCache.Add(infraction);
        SaveJsonCache();
    }

    public static IReadOnlyList<InfractionData> GetInfractions(string offenderId)
    {
        return _jsonCache
            .FindAll(i => i.OffenderId == offenderId)
            .AsReadOnly();
    }
}
