using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsInfractionSystem.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class ViewInfractionsCommand : ICommand
{
    public string Command { get; } = "viewinfractions";
    public string[] Aliases { get; } = ["vinfractions", "vinfraction", "viewf", "viewfs", "vf", "vfs", "viewinfraction"];
    public string Description { get; } = "View a player's infractions";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions(KittsInfractionSystem.Config.WarningPermission))
        {
            response = "<color=red>You do not have the required permissions.</color>";
            return false;
        }

        if (arguments.Count < 1)
        {
            response = "<color=orange>Correct usage: viewinfraction <playerId | userId></color>";
            return false;
        }

        string targetArg = arguments.At(0);
        string offenderId;

        if (int.TryParse(targetArg, out int playerId) && Player.TryGet(playerId, out Player targetPlayer))
            offenderId = targetPlayer.UserId;
        else
        {
            offenderId = targetArg;

            if (!offenderId.EndsWith("@steam", StringComparison.OrdinalIgnoreCase))
                offenderId += "@steam";
        }

        response = $"<color=#00FFFF>Infractions for {InfractionManager.GetInfractions(offenderId).First().OffenderName} ({offenderId})</color>\n" +
            $"{InfractionManager.GetPrettyColouredInfractions(offenderId)}";

        return true;
    }
}
