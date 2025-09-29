using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using ServerCoreBySeen.Configs;

namespace ServerCoreBySeen.Helpers;

public class GameHelper
{
    public static List<CCSPlayerController> GetPlayersByTeam(CsTeam team)
    {
        return [.. Utilities.GetPlayers().Where(p => p != null && p.IsValid && p.Team == team)];
    }

    public static bool IsPlayerVip(ulong SteamId, MasterConfig config)
    {
        return config.Permissions.VipSteamIds.Contains(SteamId);
    }

    public static bool IsPlayerAdmin(ulong SteamId, MasterConfig config)
    {
        return config.Permissions.AdminSteamIds.Contains(SteamId);
    }
}