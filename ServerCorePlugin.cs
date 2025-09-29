using CounterStrikeSharp.API.Core;
using ServerCoreBySeen.Configs;
using ServerCoreBySeen.Database;
using ServerCoreBySeen.Handlers;

namespace ServerCoreBySeen;

public class ServerCorePlugin : BasePlugin, IPluginConfig<Configs.MasterConfig>
{
    public override string ModuleAuthor => "Seen";
    public override string ModuleName => "ServerCoreBySeen";
    public override string ModuleVersion => "1.0.0";

    public MasterConfig Config { get; set; } = new();

    private RankingDatabase _rankingDatabase = null!;
    private RankingHandler _rankingHandler = null!;
    private PlayerHandler _playerHandler = null!;


    public override void Load(bool hotReload)
    {
        _rankingDatabase = new RankingDatabase(Config);
        _rankingDatabase.Initialize(ModuleDirectory);

        _rankingHandler = new RankingHandler(_rankingDatabase, Config);
        _playerHandler = new PlayerHandler(Config);

        RegisterEventHandler<EventPlayerConnectFull>(_playerHandler.OnPlayerConnectFull);

        if (Config.General.EnableRanking)
        {
            RegisterEventHandler<EventPlayerConnectFull>(_rankingHandler.OnPlayerConnectFullHandler);
            RegisterEventHandler<EventPlayerDeath>(_rankingHandler.OnPlayerDeathHandler);
            RegisterEventHandler<EventBombDefused>(_rankingHandler.OnBombDefusedHandler);
            RegisterEventHandler<EventRoundEnd>(_rankingHandler.OnRoundEndHandler);
            RegisterEventHandler<EventBombPlanted>(_rankingHandler.OnBombPlantedHandler);
        }

        Console.WriteLine("[ServerCoreBySeen] - loaded!");
    }

    public override void Unload(bool hotReload)
    {
        _rankingDatabase.Dispose();
        base.Unload(hotReload);
    }


    public void OnConfigParsed(MasterConfig config)
    {
        Config = config;
        Console.WriteLine("[ServerCoreBySeen] - Config parsed successfully!");
    }


}
