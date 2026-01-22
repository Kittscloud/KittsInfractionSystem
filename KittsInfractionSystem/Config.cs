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

    /// <summary>
    /// MongoDB URI if using database.
    /// </summary>
    [Description("MongoDB URI if using database")]
    public string MongoDB_uri { get; set; } = "mongodb://username:password@ip:port/";

    /// <summary>
    /// MongoDB name if using database.
    /// </summary>
    [Description("MongoDB name if using database")]
    public string MongoDB_name { get; set; } = "KittsInfractionSystem";

    /// <summary>
    /// Should save to MongoDB, saves to JSON if flase, does not migrate data.
    /// </summary>
    [Description("Should save to MongoDB, saves to JSON if flase, does not migrate data")]
    public bool UseMongoDB { get; set; } = false;

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
