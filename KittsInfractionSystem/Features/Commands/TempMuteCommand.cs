using CommandSystem;
using KittsInfractionSystem.Features.Enums;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsInfractionSystem.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class TempMuteCommand : ICommand
{
    public string Command { get; } = "tempmute";
    public string[] Aliases { get; } = ["tm", "tmute", "tempm"];
    public string Description { get; } = "Tempmutes a Player";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Player.TryGet(sender, out Player player))
        {
            response = "<color=red>You must be a player to run this command.</color>";
            return false;
        }

        if (!player.HasPermissions(KittsInfractionSystem.Config.TempMutePermission))
        {
            response = "<color=red>You do not have the required permissions.</color>";
            return false;
        }

        if (arguments.Count < 3)
        {
            response = "<color=orange>Correct usage: tempmute <playerId> <duration> <reason></color>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player targetPlayer))
        {
            response = "<color=red>Invalid Player.</color>\n<color=#00FFFF>Please use a valid playerId.</color>";
            return false;
        }

        // Parse duration
        if (!TryParseDuration(arguments.At(1), out TimeSpan duration))
        {
            response = "<color=red>Invalid duration format.</color>\n" +
                "<color=#00FFFF>Examples: 10s, 1minute, 2h, 1day</color>";
            return false;
        }
        
        if (targetPlayer.IsMuted || InfractionManager.TryGetTempMute(targetPlayer.UserId, out DateTime _))
        {
            response = "<color=red>Player is already muted.</color>";
            return false;
        }

        string reason = string.Join(" ", arguments.Skip(2));

        InfractionManager.AddTempMute(targetPlayer.UserId, duration);

        InfractionManager.AddInfraction(
            InfractionType.TempMute,
            targetPlayer.UserId,
            targetPlayer.DisplayName,
            player.UserId,
            player.DisplayName,
            reason,
            duration
        );

        response = $"<color=green>{targetPlayer.DisplayName} has been temporarily muted for {duration}.</color>";
        return true;
    }

    private static bool TryParseDuration(string input, out TimeSpan duration)
    {
        duration = TimeSpan.Zero;
        input = input.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Split into number + unit
        int index = 0;
        while (index < input.Length && (char.IsDigit(input[index]) || input[index] == '.'))
            index++;

        if (index == 0)
            return false;

        string numberPart = input[..index];
        string unitPart = input[index..].Trim();

        if (!double.TryParse(numberPart, out double value))
            return false;

        // Determine multiplier
        switch (unitPart)
        {
            case "s":
            case "sec":
            case "secs":
            case "second":
            case "seconds":
                duration = TimeSpan.FromSeconds(value);
                return true;

            case "m":
            case "min":
            case "mins":
            case "minute":
            case "minutes":
                duration = TimeSpan.FromMinutes(value);
                return true;

            case "h":
            case "hr":
            case "hrs":
            case "hour":
            case "hours":
                duration = TimeSpan.FromHours(value);
                return true;

            case "d":
            case "day":
            case "days":
                duration = TimeSpan.FromDays(value);
                return true;

            default:
                return false; // Unknown unit
        }
    }
}
