# KittsInfractionSystem
*LabAPI Infraction Tool*

[![License](https://img.shields.io/badge/License-AGPL%20v3.0-blue?style=for-the-badge)](https://github.com/Kittscloud/KittsInfractionSystem/blob/main/LICENSE)
[![Downloads](https://img.shields.io/github/downloads/Kittscloud/KittsInfractionSystem/total?style=for-the-badge)](https://github.com/Kittscloud/ServerSpecificsSyncer/releases/latest)
[![GitHub release](https://img.shields.io/github/v/release/Kittscloud/KittsInfractionSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsInfractionSystem/releases/latest)
[![](https://img.shields.io/badge/.NET-4.8.1-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481)
[![GitHub stars](https://img.shields.io/github/stars/Kittscloud/KittsInfractionSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsInfractionSystem/stargazers)
[![GitHub issues](https://img.shields.io/github/issues/Kittscloud/KittsInfractionSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsInfractionSystem/issues)

`KittsInfractionSystem` is a tool that adds warning, tempmuting and infraction tracking to `SCP Secret Laboratory` using `LabAPI`.

## Consider Supporting?
If you enjoy this project and would like to support future development, I would greatly appreciate it if you considered donating via my [`Ko-Fi`](https://ko-fi.com/kittscloud).

## How to use KittsInfractionSystem:
There are two options when it comes to installing `KittsInfractionSystem`, normal and MongoDB.
The difference between the two is that normal stores everything in a `.JSON` and MongoDB stores everything in a Mongo database. 

To install `KittsInfractionSystem` on your server, you will need:
- `Newtonsoft.Json` `v13.0.4` or later.
- `KittsInfractionSystem` latest verion.

To install `KittsInfractionSystem` with MonogDB on your server, you will need:
- `DnsClient` `v1.6.1` or later.
- `Microsoft.Extensions.Logging.Abstractions` `v2.0.0` or later.
- `MognoDB.Bson` `v3.6.0` or later.
- `MongoDB.Driver` `v3.6.0` or later.
- `Newtonsoft.Json` `v13.0.4` or later.
- `KittsInfractionSystem` latest verion.

All of these files can be found in the [`latest release`](https://github.com/Kittscloud/KittsInfractionSystem/releases/latest).

Once you have these:
- Place `DnsClient.dll` in the `dependencies` folder, if applicable.
- Place `Microsoft.Extensions.Logging.Abstractions.dll` in the `dependencies` folder, if applicable.
- Place `MognoDB.Bson.dll` in the `dependencies` folder, if applicable.
- Place `MognoDB.Driver.dll` in the `dependencies` folder, if applicable.
- Place `Newtonsoft.Json.dll` in the `dependencies` folder.
- Place `KittsInfractionSystem.dll` in the `plugins` folder.

Run the server and you're set!

### Configurations:
| Parameter                  | Type     | Description                                                            | Default Value                            |
|----------------------------|----------|------------------------------------------------------------------------|------------------------------------------|
| `IsEnabled`                | `bool`   | Is plugin enabled.                                                     | `true`                                   |
| `Debug`                    | `bool`   | Sends debug logs to console.                                           | `false`                                  |
| `MongoDBURI`               | `string` | MongoDB URI.                                                           | `"mongodb://username:password@ip:port/"` |
| `MongoDBName`              | `string` | MongoDB name.                                                          | `KittsInfractionSystem`                  |
| `MongoDBCollectionName`    | `string` | MongoDB collection name if using database.                             | `InfractionData`                         |
| `WarningPermission`        | `string` | Permission for warn command.                                           | `"kts.warn"`                             |
| `TempMutePermission`       | `string` | Permission for temp muting command.                                    | `"kts.tempmute"`                         |
| `ViewInfractionPermission` | `string` | Permission for viewing infraction command.                             | `"kts.viewinfractions"`                  |

### Default YML Config File
```yml
# Is plugin enabled
is_enabled: true
# Sends debug logs to console
debug: false
# Permission for warn command
warning_permission: kts.warn
# Permission for temp muting command
temp_mute_permission: kts.tempmute
# Permission for viewing infraction command
view_infraction_permission: kts.viewinfractions
```

### MongoDB YML Config File
```yml
# Is plugin enabled
is_enabled: true
# Sends debug logs to console
debug: false
# MongoDB URI
mongo_d_b_u_r_i: mongodb://username:password@ip:port/
# MongoDB name
mongo_d_b_name: KittsInfractionSystem
# MongoDB collection name
mongo_d_b_collection_name: InfractionData
# Permission for warn command
warning_permission: kts.warn
# Permission for temp muting command
temp_mute_permission: kts.tempmute
# Permission for viewing infraction command
view_infraction_permission: kts.viewinfractions
```

### Want to use in your own project?
To install in your project, simply reference the `KittsInfractionSystem.dll` file, found in the [`latest release`](https://github.com/Kittscloud/KittsMenuSystem/releases/latest).

`KittsInfractionSystem.dll` is mainly a tool and does not have much to offer when referencing, the most important part is the `OnInfractionAdded` action which is called when an infraction is added.

### InfractionManager Class
| Parameter / Method                                   | Type / Return Type              | Description                                                     |
|------------------------------------------------------|---------------------------------|-----------------------------------------------------------------|
| `OnInfractionAdded`                                  | `Action<InfractionData>`        | `Action` called when an infraction is added.                    |
| `AddInfraction(7 params)`                            | `void`                          | Adds a new infraction to the database.                          |
| `GetInfractions(string)`                             | `IReadOnlyList<InfractionData>` | Gets a list of offenderId's infractions.                        |
| `GetPrettyInfraction(InfractionData)`                | `string`                        | Gets a pretty string of target `InfractionData`.                |
| `GetPrettyInfractions(List<InfractionData>)`         | `string`                        | Gets a pretty string from a list of `InfractionDatas`.          |
| `GetPrettyInfractions(string)`                       | `string`                        | Gets a pretty string of offenderId's infractions.               |
| `GetPrettyColouredInfraction(InfractionData)`        | `string`                        | Gets a pretty coloured string of target `InfractionData`.       |
| `GetPrettyColouredInfractions(List<InfractionData>)` | `string`                        | Gets a pretty coloured string from a list of `InfractionDatas`. |
| `GetPrettyColouredInfractions(string)`               | `string`                        | Gets a pretty coloured string of offenderId's infractions.      |
| `AddTempMute(string, TimeSpan)`                      | `void`                          | Temporarily mute a player.                                      |
| `TryGetTempMute(string, out DateTime)`               | `bool`                          | Trys to get the `DateTime` at which the user unmutes.           |
| `RemoveTempMute(string)`                             | `void`                          | Removes a user from being temporarily muted.                    |

I have decided to not include an exmaple as it's VERY easy to use, just attach your own function to the action as shown below

```cssharp
public void OnInfractionAdded(InfractionData data)
{
    // Do something when infraction added
    // For exmaple you can log the data to the console
}

// Make sure to attach the function to the action somewhere
InfractionManager.InfractionAdded += OnInfractionAdded;
```

## Found a bug or have feedback?
If you have found a bug please make an issue on GitHub or the quickest way is to message me on discord at `kittscloud`.

Also message me on discord if you have feedback for me, I'd appreciate it very much. Thank you!
