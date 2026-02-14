# KittsInfractionSystem Changelog

## Version 0.3.1
- Added `NuGet Package` - Can now use the NuGet package.
- Updated `InfractionType` enum - Now inherits a `ushort`.

## Version 0.3.0
- Added `Jail` command - Used for jailing players.
- Added `Unjail` command - Used for unjailing players.
- Added `JailEvents` - Used for removing a player from `JailPlayers` if they leave while jailed and clearing on round restart.
- Added `JailedPlayers` dictionary - Stores all the jailed players.
- Added `TryJail` function - Trys to jail a player, returning false if player is already jailed.
- Added `TryUnjail` function - Trys to unjail a player, returning false if player is not jailed.
- Added `JailData` - Model used when players are jailed.
- Added `JailPermission` config - Used for jail and unjail command.
- Updated `_tempMutes` dictionary - Now named `TempMutes` and is now public with internal set.

## Version 0.2.0
- Added `GetPrettyInfraction` function - Gets a pretty string for an `InfractionData`.
- Added `GetPrettyInfractions` function - Gets a pretty string from a list of `InfractionData` or by an offender's id.
- Added `GetPrettyColouredInfraction` function - Gets a pretty coloured string for an `InfractionData`.
- Added `GetPrettyColouredInfractions` function - Gets a pretty coloured string from a list of `InfractionData` or by an offender's id.
- Updated `FormatDuration` function - Now returns time down to the second.
- Updated `KittsInfractionSystem` - Now split into a normal and MongoDB version.

## Version 0.1.1
- Updated `KittsInfractionSystem` class - Now `public` instead of `internal`, rookie mistake.
- Updated `Dependencies` - Removed unnecessary dependencies.
