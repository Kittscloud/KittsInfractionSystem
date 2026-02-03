using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace KittsInfractionSystem.Features.Events;

internal sealed class JailEvents : CustomEventsHandler
{
    public override void OnServerRoundRestarted() =>
        InfractionManager.JailedPlayers.Clear();

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) =>
        InfractionManager.JailedPlayers.Remove(ev.Player.PlayerId);
}
