using KittsInfractionSystem.Features.Database;
using KittsInfractionSystem.Features.Enums;
using KittsInfractionSystem.Features.Models;
using LabApi.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KittsInfractionSystem.Features;

public class InfractionManager
{
    private static Dictionary<string, DateTime> _tempMute = [];
    private static string TempMuteFilePath =>
        Path.Combine(KittsInfractionSystem.Instance.GetConfigDirectory().FullName, "TempMutes.json");

    #region Adding Infraction
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

#if MONGODB
            DatabaseMongo.AddInfraction(infraction);
#else
            DatabaseJson.AddInfraction(infraction);
#endif

        InfractionAdded?.Invoke(infraction);

        Log.Debug("Database.AddInfraction", $"{moderatorName} ({moderatorId}) added infraction for {offenderName} ({offenderId})");
    }
    #endregion

    #region Getting Infractions
    /// <summary>
    /// Gets a list of offenderId's <see cref="InfractionData"/>.  
    /// </summary>
    /// <param name="offenderId">The offender's Id.</param>
    /// <returns></returns>
    public static IReadOnlyList<InfractionData> GetInfractions(string offenderId)
    {
#if MONGODB
        return DatabaseMongo.GetInfractions(offenderId);
#else
        return DatabaseJson.GetInfractions(offenderId);
#endif
    }

    /// <summary>
    /// Gets a pretty string of target <see cref="InfractionData"/>.
    /// </summary>
    /// <param name="infractionData">Target <see cref="InfractionData"/>.</param>
    /// <returns>Pretty string of Target <see cref="InfractionData"/></returns>
    public static string GetPrettyInfraction(InfractionData infractionData)
    {
        string duration = infractionData.Duration.HasValue
            ? FormatDuration(infractionData.Duration.Value)
            : "Permanent";

        string reason = string.IsNullOrWhiteSpace(infractionData.ReasonAndEvidence)
            ? "No reason provided"
            : infractionData.ReasonAndEvidence;

        return $"{infractionData.Type} | Issued: {infractionData.Issued:yyyy-MM-dd HH:mm} | " +
            $"Moderator: {infractionData.ModeratorName} ({infractionData.ModeratorId}) | " +
            $"Duration: {duration} | Reason: {reason}";
    }

    /// <summary>
    /// Gets a pretty string from a list of <see cref="InfractionData"/>s.
    /// </summary>
    /// <param name="infractionDatas">List of <see cref="InfractionData"/>s.</param>
    /// <returns>Pretty string from the list of <see cref="InfractionData"/>s</returns>
    public static string GetPrettyInfractions(List<InfractionData> infractionDatas)
    {
        if (infractionDatas.Count == 0)
            return "No infractions found.";

        StringBuilder sb = new();
        int index = 1;

        foreach (InfractionData i in infractionDatas)
        {
            sb.AppendLine($"[{index}] {GetPrettyInfraction(i)}");
            index++;
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Gets a pretty string of offenderId's infractions.
    /// </summary>
    /// <param name="offenderId">The offender's Id.</param>
    /// <returns>Pretty string of offender's infractions</returns>
    public static string GetPrettyInfractions(string offenderId) =>
        GetPrettyInfractions([.. GetInfractions(offenderId)]);

    /// <summary>
    /// Gets a pretty coloured string of target <see cref="InfractionData"/>.
    /// </summary>
    /// <param name="infractionData">Target <see cref="InfractionData"/>.</param>
    /// <returns>Pretty coloured string of Target <see cref="InfractionData"/></returns>
    public static string GetPrettyColouredInfraction(InfractionData infractionData)
    {
        string duration = infractionData.Duration.HasValue
                ? FormatDuration(infractionData.Duration.Value)
                : "<color=red>Permanent</color>";

        string reason = string.IsNullOrWhiteSpace(infractionData.ReasonAndEvidence)
            ? "<color=grey>No reason provided</color>"
            : $"<color=white>{infractionData.ReasonAndEvidence}</color>";

        return $"<color=yellow>{infractionData.Type}</color> | " +
            $"<color=#00FFFF>{infractionData.Issued:yyyy-MM-dd HH:mm}</color> | " +
            $"<color=#FFA500>{infractionData.ModeratorName}</color> <color=grey>({infractionData.ModeratorId})</color> | " +
            $"<color=#FF5555>{duration}</color> | " +
            $"<color=#00FF00>Reason:</color> {reason}";
    }

    /// <summary>
    /// Gets a pretty coloured string from a list of <see cref="InfractionData"/>s.
    /// </summary>
    /// <param name="infractionDatas">List of <see cref="InfractionData"/>s.</param>
    /// <returns>Pretty coloured string from the list of <see cref="InfractionData"/>s</returns>
    public static string GetPrettyColouredInfractions(List<InfractionData> infractionDatas)
    {
        if (infractionDatas.Count == 0)
            return "<color=green>No infractions found.</color>";

        StringBuilder sb = new();
        int index = 1;

        foreach (InfractionData i in infractionDatas)
        {
            sb.AppendLine($"<color=#AAAAAA>[{index}]</color> {GetPrettyColouredInfraction(i)}");
            index++;
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Gets a pretty coloured string of offenderId's infractions.
    /// </summary>
    /// <param name="offenderId">The offender's Id.</param>
    /// <returns>Pretty coloured string of offender's infractions</returns>
    public static string GetPrettyColouredInfractions(string offenderId) =>
        GetPrettyColouredInfractions([.. GetInfractions(offenderId)]);

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            return "0 seconds";

        int days = duration.Days;
        int hours = duration.Hours;
        int minutes = duration.Minutes;
        int seconds = duration.Seconds;

        List<string> parts = [];

        if (days > 0) parts.Add($"{days} day(s)");
        if (hours > 0) parts.Add($"{hours} hour(s)");
        if (minutes > 0) parts.Add($"{minutes} minute(s)");
        if (seconds > 0) parts.Add($"{seconds} second(s)");

        return string.Join(", ", parts);
    }
    #endregion

    #region TempMuting
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
    #endregion
}
