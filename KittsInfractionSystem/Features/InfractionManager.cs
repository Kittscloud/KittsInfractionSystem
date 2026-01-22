using KittsInfractionSystem.Features.Database;
using KittsInfractionSystem.Features.Enums;
using KittsInfractionSystem.Features.Models;
using LabApi.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace KittsInfractionSystem.Features;

public class InfractionManager
{
    private static Dictionary<string, DateTime> _tempMute = [];
    private static string TempMuteFilePath =>
        Path.Combine(KittsInfractionSystem.Instance.GetConfigDirectory().FullName, "TempMutes.json");

    /// <summary>
    /// Action called when an infraction is added.
    /// </summary>
    public static event Action<InfractionData> InfractionAdded;

    /// <summary>
    /// Adds a new infraction to the database.
    /// </summary>
    /// <param name="infractionType">Type of infraction.</param>
    /// <param name="offenderId">Offender's Id.</param>
    /// <param name="offenderName">Offender's Name.</param>
    /// <param name="moderatorId">Moderator's Id.</param>
    /// <param name="moderatorName">Moderator's Name.</param>
    /// <param name="reasonAndEvidence">Reason for infraction.</param>
    /// <param name="duration">Duration of infraciont.</param>
    public static void AddInfraction(
        InfractionType infractionType,
        string offenderId,
        string offenderName,
        string moderatorId,
        string moderatorName,
        string reasonAndEvidence = null,
        TimeSpan? duration = null)
    {
        InfractionData infraction = new()
        {
            Type = Enum.GetName(typeof(InfractionType), infractionType),
            OffenderName = offenderName,
            OffenderId = offenderId,
            ModeratorName = moderatorName,
            ModeratorId = moderatorId,
            ReasonAndEvidence = reasonAndEvidence,
            Duration = duration
        };

        if (KittsInfractionSystem.Config.UseMongoDB)
            DatabaseMongo.AddInfraction(infraction);
        else
            DatabaseJson.AddInfraction(infraction);

        InfractionAdded?.Invoke(infraction);

        Log.Debug("Database.AddInfraction", $"{moderatorName} ({moderatorId}) added infraction for {offenderName} ({offenderId})");
    }

    /// <summary>
    /// Gets a list of offenderId's infractions.  
    /// </summary>
    /// <param name="offenderId">The offender's Id.</param>
    /// <returns></returns>
    public static IReadOnlyList<InfractionData> GetInfractions(string offenderId)
    {
        if (KittsInfractionSystem.Config.UseMongoDB)
            return DatabaseMongo.GetInfractions(offenderId);
        else
            return DatabaseJson.GetInfractions(offenderId);
    }

    /// <summary>
    /// Gets a pretty string of offenderId's infractions.
    /// </summary>
    /// <param name="offenderId">The offender's Id.</param>
    /// <returns></returns>
    public static string GetPrettyInfractions(string offenderId)
    {
        IReadOnlyList<InfractionData> infractions = GetInfractions(offenderId);

        if (infractions.Count == 0)
            return "No infractions found.";

        var sb = new System.Text.StringBuilder();
        int index = 1;

        foreach (InfractionData i in infractions)
        {
            string duration = i.Duration.HasValue
                ? FormatDuration(i.Duration.Value)
                : "Permanent";

            string reason = string.IsNullOrWhiteSpace(i.ReasonAndEvidence)
                ? "No reason provided"
                : i.ReasonAndEvidence;

            sb.AppendLine(
                $"[{index}] {i.Type} | Issued: {i.Issued:yyyy-MM-dd HH:mm} | " +
                $"Moderator: {i.ModeratorName} ({i.ModeratorId}) | " +
                $"Duration: {duration} | Reason: {reason}"
            );

            index++;
        }

        return sb.ToString().TrimEnd();
    }
    
    /// <summary>
     /// Gets a pretty coloured string of offenderId's infractions.
     /// </summary>
     /// <param name="offenderId">The offender's Id.</param>
     /// <returns></returns>
    public static string GetPrettyColouredInfractions(string offenderId)
    {
        IReadOnlyList<InfractionData> infractions = GetInfractions(offenderId);

        if (infractions.Count == 0)
            return "<color=green>No infractions found.</color>";

        var sb = new System.Text.StringBuilder();
        int index = 1;

        foreach (InfractionData i in infractions)
        {
            string duration = i.Duration.HasValue
                ? FormatDuration(i.Duration.Value)
                : "<color=red>Permanent</color>";

            string reason = string.IsNullOrWhiteSpace(i.ReasonAndEvidence)
                ? "<color=grey>No reason provided</color>"
                : $"<color=white>{i.ReasonAndEvidence}</color>";

            sb.AppendLine(
                $"<color=#AAAAAA>[{index}]</color> " +
                $"<color=yellow>{i.Type}</color> | " +
                $"<color=#00FFFF>{i.Issued:yyyy-MM-dd HH:mm}</color> | " +
                $"<color=#FFA500>{i.ModeratorName}</color> <color=grey>({i.ModeratorId})</color> | " +
                $"<color=#FF5555>{duration}</color> | " +
                $"<color=#00FF00>Reason:</color> {reason}"
            );

            index++;
        }

        return sb.ToString().TrimEnd();
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
            return $"{(int)duration.TotalDays} day(s)";

        if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours} hour(s)";

        if (duration.TotalMinutes >= 1)
            return $"{(int)duration.TotalMinutes} minute(s)";

        return $"{(int)duration.TotalSeconds} second(s)";
    }

    internal static void InitTempMutes()
    {
        EnsureTempMuteFile();
        LoadTempMutes();
    }

    internal static void SaveTempMutes()
    {
        try
        {
            File.WriteAllText(TempMuteFilePath, JsonConvert.SerializeObject(_tempMute, Formatting.Indented));
        }
        catch (Exception e)
        {
            Log.Error("InfractionManager.SaveTempMutes", $"Error saving temp mutes: {e.Message}");
            Log.Debug("InfractionManager.SaveTempMutes", e.ToString());
        }
    }

    internal static void EnsureTempMuteFile()
    {
        string dir = KittsInfractionSystem.Instance.GetConfigDirectory().FullName;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(TempMuteFilePath))
            File.WriteAllText(TempMuteFilePath, "{}");
    }

    internal static void LoadTempMutes()
    {
        try
        {
            var json = File.ReadAllText(TempMuteFilePath);
            Dictionary<string, DateTime> tempMenu = JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(File.ReadAllText(TempMuteFilePath));

            _tempMute = tempMenu;
        }
        catch (Exception e)
        {
            Log.Error("InfractionManager.LoadTempMutes", $"Error loading temp mutes: {e.Message}");
            Log.Debug("InfractionManager.LoadTempMutes", e.ToString());
        }
    }

    /// <summary>
    /// Temporarily mute a player.
    /// </summary>
    /// <param name="userId">The player's userId.</param>
    /// <param name="duration">How long to mute the player.</param>
    public static void AddTempMute(string userId, TimeSpan duration)
    {
        DateTime unmuteAt = DateTime.Now + duration;

        _tempMute[userId] = unmuteAt;
        SaveTempMutes();
        Log.Debug("InfractionManager.AddTempMute", $"Temp muted {userId} until {unmuteAt}");
    }

    /// <summary>
    /// Trys to get the <see cref="DateTime"/> at which the user unmutes.
    /// </summary>
    /// <param name="userId">The user's Id.</param>
    /// <param name="unmuteAt">The <see cref="DateTime"/> at which the user unmutes.</param>
    /// <returns>If the user is temporarily muted.</returns>
    public static bool TryGetTempMute(string userId, out DateTime unmuteAt) =>
        _tempMute.TryGetValue(userId, out unmuteAt);

    /// <summary>
    /// Removes a user from being temporarily muted.
    /// </summary>
    /// <param name="userId">User's Id to unmute.</param>
    public static void RemoveTempMute(string userId)
    {
        if (_tempMute.Remove(userId))
        {
            SaveTempMutes();

            Log.Debug("InfractionManager.RemoveTempMute", $"{userId} is no longer muted.");
        }
    }
}
