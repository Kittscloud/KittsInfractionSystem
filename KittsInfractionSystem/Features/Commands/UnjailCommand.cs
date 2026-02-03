using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Text;

namespace KittsInfractionSystem.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class UnjailCommand : ICommand
{
    public string Command { get; } = "unjail";
    public string[] Aliases { get; } = ["unj"];
    public string Description { get; } = "Unjails a player and gives back location, role and inventory";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Player.TryGet(sender, out Player player))
        {
            response = "<color=red>You must be a player to run this command.</color>";
            return false;
        }

        if (!player.HasPermissions(KittsInfractionSystem.Config.JailPermission))
        {
            response = "<color=red>You do not have the required permissions.</color>";
            return false;
        }

        if (arguments.Count < 1)
        {
            response = "<color=orange>Correct usage: unjail <playerId> [playerId2] [playerId3]...</color>";
            return false;
        }

        StringBuilder sb = new();
        sb.Append("\n");

        int successCount = 0;

        foreach (string arg in arguments)
        {
            if (!int.TryParse(arg, out int id) || !Player.TryGet(id, out Player targetPlayer))
            {
                sb.AppendLine($"<color=red>Invalid PlayerID ({arg})</color>");
                continue;
            }

            if (!InfractionManager.TryUnjail(targetPlayer))
            {
                sb.AppendLine($"<color=red>{targetPlayer.DisplayName} is not currently jailed.</color>");
                continue;
            }

            sb.Append($"<color=green>Successfully unjailed {targetPlayer.DisplayName}.</color>\n");
            successCount++;
        }

        response = sb.ToString().TrimEnd();

        Log.Debug("UnjailCommand", $"{response} {successCount > 0}");
        return successCount > 0;
    }
}
