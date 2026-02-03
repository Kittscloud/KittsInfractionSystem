using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Text;

namespace KittsInfractionSystem.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class JailCommand : ICommand
{
    public string Command { get; } = "jail";
    public string[] Aliases { get; } = ["j"];
    public string Description { get; } = "Jails a player to the tutorial tower, remembers location, role and inventory";

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
            response = "<color=orange>Correct usage: jail <playerId> [playerId2] [playerId3]...</color>";
            return false;
        }

        StringBuilder sb = new();
        sb.Append("\n");

        int successCount = 0;

        foreach (string arg in arguments)
        {
            if (!int.TryParse(arg, out int id) || !Player.TryGet(id, out Player targetPlayer))
            {
                sb.AppendLine($"<color=red>Invalid PlayerID ({id})</color>");
                continue;
            }

            if (!InfractionManager.TryJail(targetPlayer))
            {
                sb.AppendLine($"<color=red>{targetPlayer.DisplayName} is already jailed.</color>");
                continue;
            }

            sb.AppendLine($"<color=green>Successfully jailed {targetPlayer.DisplayName}.</color>");
            successCount++;
        }

        response = sb.ToString().TrimEnd();

        Log.Debug("JailCommand", $"{response} {successCount > 0}");
        return successCount > 0;
    }
}
