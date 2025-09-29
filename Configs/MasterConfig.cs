using CounterStrikeSharp.API.Core;

namespace ServerCoreBySeen.Configs;

public class MasterConfig : BasePluginConfig
{
    public GeneralConfig General { get; set; } = new();
    public ScoreBoardConfig Scoreboard { get; set; } = new();
    public VipConfig Vip { get; set; } = new();
    public RankingSystemConfig Ranking { get; set; } = new();
}

public class GeneralConfig
{
    public string ChatServerMessagesPrefix { get; set; } = "[Server Core By Seen]";
    public bool EnableVip { get; set; } = true;
    public bool EnableRanking { get; set; } = true;
    public bool EnableScoreboard { get; set; } = true;
    public string AdminMenuCommand { get; set; } = "admin";
}


public class ScoreBoardConfig
{
    public string TopOneTag { get; set; } = "[TOP-1]";
    public string VipTag { get; set; } = "[VIP]";
    public string AdminTag { get; set; } = "[ADMIN]";
    public string DevTag { get; set; } = "[DEV]";
}

public class VipConfig
{
    public List<ulong> SteamIds { get; set; } = [];
    public int ExtraExp { get; set; } = 100;
    public int ExtraMoney { get; set; } = 500;
    public int ExtraMoneyFromRound { get; set; } = 3;
    public bool ShowTag { get; set; } = true;
}

public class RankingSystemConfig
{
    public int ExpPerKill { get; set; } = 20;
    public int ExpPerHeadshot { get; set; } = 10;
    public int ExpPerBombDefuse { get; set; } = 50;
    public int ExpPerBombPlant { get; set; } = 30;
    public int ExpPerRoundWin { get; set; } = 40;
    public int ExpLostPerRoundLoss { get; set; } = 10;
    public int ExpLostPerDeath { get; set; } = 5;
    public int MaxLevel { get; set; } = 0;
    public bool ShowTopOneTag { get; set; } = true;
}