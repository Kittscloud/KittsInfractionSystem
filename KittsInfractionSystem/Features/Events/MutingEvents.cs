using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using System;

namespace KittsInfractionSystem.Features.Events;

internal sealed class MutingEvents : CustomEventsHandler
{
    public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (InfractionManager.TryGetTempMute(ev.Player.UserId, out DateTime unmuteAt))
        {
            if (DateTime.Now >= unmuteAt)
                InfractionManager.RemoveTempMute(ev.Player.UserId);
            else
            {
                ev.Player.SendHint($"<color=red>You are currently muted until {unmuteAt.ToLongDateString()} {unmuteAt.ToLongTimeString()}</color>", 0.1f);

                ev.IsAllowed = false;
            }
        }
    }

    public override void OnPlayerUsingIntercom(PlayerUsingIntercomEventArgs ev)
    {
        if (InfractionManager.TryGetTempMute(ev.Player.UserId, out DateTime unmuteAt))
        {
            if (DateTime.Now >= unmuteAt)
                InfractionManager.RemoveTempMute(ev.Player.UserId);
            else
            {
                ev.Player.SendHint($"<color=red>You are currently muted until {unmuteAt.ToLongDateString()} {unmuteAt.ToLongTimeString()}</color>", 0.1f);

                ev.IsAllowed = false;
            }
        }
    }

    public override void OnPlayerUnmuted(PlayerUnmutedEventArgs ev) =>
        InfractionManager.RemoveTempMute(ev.Player.UserId);
}
