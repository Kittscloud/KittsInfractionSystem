using System.ComponentModel;

namespace KittsInfractionSystem;

public class Config
{
    /// <summary>
    /// Is plugin enabled.
    /// </summary>
    [Description("Is plugin enabled")]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Sends debug logs to console.
    /// </summary>
    [Description("Sends debug logs to console")]
    public bool Debug { get; set; } = false;

#if MONGODB
    /// <summary>
    /// MongoDB URI.
    /// </summary>
    [Description("MongoDB URI")]
    public string MongoDBURI { get; set; } = "mongodb://username:password@ip:port/";

    /// <summary>
    /// MongoDB name.
    /// </summary>
    [Description("MongoDB name")]
    public string MongoDBName { get; set; } = "KittsInfractionSystem";

    /// <summary>
    /// MongoDB collection name.
    /// </summary>
    [Description("MongoDB collection name")]
    public string MongoDBCollectionName { get; set; } = "InfractionData";
#endif

    /// <summary>
    /// Permission for warn command.
    /// </summary>
    [Description("Permission for warn command")]
    public string WarningPermission { get; set; } = "kts.warn";

    /// <summary>
    /// Permission for temp muting command.
    /// </summary>
    [Description("Permission for temp muting command")]
    public string TempMutePermission { get; set; } = "kts.tempmute";

    /// <summary>
    /// Permission for temp muting command.
    /// </summary>
    [Description("Permission for viewing infraction command")]
    public string ViewInfractionPermission { get; set; } = "kts.viewinfractions";
}
