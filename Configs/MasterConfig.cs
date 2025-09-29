using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace ServerCoreBySeen.Configs;

public class MasterConfig : BasePluginConfig
{
    public GeneralConfig General { get; set; } = new();
    public ScoreBoardConfig Scoreboard { get; set; } = new();
    public VipConfig Vip { get; set; } = new();
    public RankingSystemConfig Ranking { get; set; } = new();
    public PermissionsConfig Permissions { get; set; } = new();
    public MessagesConfig Messages { get; set; } = new();
}

public class GeneralConfig
{
    public bool EnableMessages { get; set; } = true;
    public bool EnableVip { get; set; } = true;
    public bool EnableRanking { get; set; } = true;
    public bool EnableTags { get; set; } = true;
    public string AdminMenuCommand { get; set; } = "admin";
}

public class PermissionsConfig
{
    public List<ulong> AdminSteamIds { get; set; } = [];
    public List<ulong> VipSteamIds { get; set; } = [];
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
    public int ExtraExp { get; set; } = 100;
    public int ExtraMoney { get; set; } = 500;
    public int ExtraMoneyFromRound { get; set; } = 3;
    public bool ShowTag { get; set; } = true;


    [JsonInclude]
    public List<string> RoundStartItems { get; set; } = new()
    {
        "healthshot",
        "weapon_flashbang",
        "weapon_smokegrenade",
        "fire_grenade",
        "weapon_hegrenade"
    };
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

public class MessagesConfig
{
    public string Prefix { get; set; } = "[Server Core By Seen]";
    public string KillPlayerMessage { get; set; } =
        "{green}You just earned {exp} XP {default}for killing a player!";
    public string HeadshotKillMessage { get; set; } =
        "{green}You just earned {exp} XP {default}for a headshot kill!";
    public string DeathMessage { get; set; } =
        "{red}You lost {exp} XP {default}for dying.";
    public string BombPlantMessage { get; set; } =
        "{green}You just earned {exp} XP {default}for planting the bomb!";
    public string BombDefuseMessage { get; set; } =
        "{green}You just earned {exp} XP {default}for defusing the bomb!";
    public string RoundWinMessage { get; set; } =
        "{green}You just earned {exp} XP {default}for winning the round!";
    public string RoundLossMessage { get; set; } =
        "{red}You lost {exp} XP {default}for losing the round.";

}