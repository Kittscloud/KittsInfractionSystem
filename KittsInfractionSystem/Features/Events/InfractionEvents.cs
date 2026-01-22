using KittsInfractionSystem.Features.Enums;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using System;

namespace KittsInfractionSystem.Features.Events;

internal sealed class InfractionEvents : CustomEventsHandler
{
    public override void OnPlayerMuted(PlayerMutedEventArgs ev) =>
        InfractionManager.AddInfraction(InfractionType.Mute, ev.Player.UserId, ev.Player.DisplayName, ev.Issuer.UserId, ev.Issuer.DisplayName, $"Muted: {ev.Player.IsMuted}, IsIntercom mute: {ev.IsIntercom}");
    public override void OnPlayerUnmuted(PlayerUnmutedEventArgs ev) =>
        InfractionManager.AddInfraction(InfractionType.Unmute, ev.Player.UserId, ev.Player.DisplayName, ev.Issuer.UserId, ev.Issuer.DisplayName, $"Unmuted: {ev.Player.IsMuted}, IsIntercom mute: {ev.IsIntercom}");

    public override void OnPlayerKicked(PlayerKickedEventArgs ev) =>
        InfractionManager.AddInfraction(InfractionType.Kick, ev.Player.UserId, ev.Player.DisplayName, ev.Issuer.UserId, ev.Issuer.DisplayName, ev.Reason);

    public override void OnPlayerBanned(PlayerBannedEventArgs ev) =>
        InfractionManager.AddInfraction(InfractionType.Ban, ev.Player?.UserId, ev.Player.DisplayName, ev.Issuer.UserId, ev.Issuer.DisplayName, ev.Reason, new TimeSpan(ev.Duration));
}
