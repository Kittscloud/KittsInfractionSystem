using CustomPlayerEffects;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KittsInfractionSystem.Features.Models;

public sealed class JailData(RoleTypeId role, Vector3 position, List<ItemType> items, Dictionary<ItemType, ushort> ammo, List<StatusEffectBase> effects)
{
    public RoleTypeId Role { get; } = role;
    public Vector3 Position { get; } = position;
    public List<ItemType> Items { get; } = items;
    public Dictionary<ItemType, ushort> Ammo { get; } = ammo;
    public List<StatusEffectBase> Effects { get; } = effects;
}
