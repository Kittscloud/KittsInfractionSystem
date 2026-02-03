using KittsInfractionSystem.Features;
using KittsInfractionSystem.Features.Database;
using KittsInfractionSystem.Features.Events;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using System;

namespace KittsInfractionSystem;

public class KittsInfractionSystem : Plugin
{
    public static Plugin Instance { get; private set; }

    public override string Name { get; } = "KittsInfractionSystem";
    public override string Author { get; } = "Kittscloud";
    public override string Description { get; } = "";
    public override LoadPriority Priority { get; } = LoadPriority.High;
    public override Version Version { get; } = new Version(0, 3, 0);
    public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);

    public static Config Config { get; set; }
    private bool _errorLoadingConfig = false;

    private readonly InfractionEvents _infractionEvents = new();
    private readonly JailEvents _jailEvents = new();
    private readonly MutingEvents _mutingEvents = new();

    public override void Enable()
    {
        Instance = this;

        if (_errorLoadingConfig)
            Log.Error("Invalid config file, check config file or generate a new one.");

        if (!Config.IsEnabled)
            return;

#if MONGODB
        DatabaseMongo.Init();
#else
        DatabaseJson.Init();
#endif

        InfractionManager.InitTempMutes();

        CustomHandlersManager.RegisterEventsHandler(_infractionEvents);
        CustomHandlersManager.RegisterEventsHandler(_jailEvents);
        CustomHandlersManager.RegisterEventsHandler(_mutingEvents);

        Log.Send($"Successfully Enabled {Name}@{Version}", colour: ConsoleColor.Green);
    }

    public override void Disable()
    {
        this.SaveConfig(Config, "config.yml");

        CustomHandlersManager.UnregisterEventsHandler(_infractionEvents);
        CustomHandlersManager.UnregisterEventsHandler(_jailEvents);
        CustomHandlersManager.UnregisterEventsHandler(_mutingEvents);

#if MONGODB
        DatabaseMongo.Stop();
#else
        DatabaseJson.Stop();
#endif

        Instance = null;

        Log.Send($"Successfully Disabled {Name}@{Version}", colour: ConsoleColor.Green);
    }

    public override void LoadConfigs()
    {
        _errorLoadingConfig = !this.TryLoadConfig("config.yml", out Config config);
        Config = config ?? new Config();

        base.LoadConfigs();
    }
}
