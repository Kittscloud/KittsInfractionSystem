using CommandSystem;
using KittsInfractionSystem.Features.Enums;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsInfractionSystem.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class WarnCommand : ICommand
{
    public string Command { get; } = "warn";
    public string[] Aliases { get; } = ["w"];
    public string Description { get; } = "Warn a Player";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Player.TryGet(sender, out Player player))
        {
            response = "<color=red>You must be a player to run this command.</color>";
            return false;
        }

        if (!player.HasPermissions(KittsInfractionSystem.Config.WarningPermission))
        {
            response = "<color=red>You do not have the required permissions.</color>";
            return false;
        }

        if (arguments.Count != 2)
        {
            response = "<color=orange>Correct usage: warn <playerId> <reason></color>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player targetPlayer))
        {
            response = "<color=red>Invalid Player.</color>\n" +
                "<color=#00FFFF>Please use a valid playerId.</color>";
            return false;
        }

        string reason = string.Join(" ", arguments.Skip(1));

        InfractionManager.AddInfraction(
            InfractionType.Warn,
            targetPlayer.UserId,
            targetPlayer.DisplayName,
            player.UserId,
            player.DisplayName,
            reason
        );

        response = $"<color=green>Successfully warned {targetPlayer.DisplayName}</color>";
        return false;
    }
}
