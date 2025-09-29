using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace ServerCoreBySeen.Helpers;

public class GameHelper
{
    public static List<CCSPlayerController> GetPlayersByTeam(CsTeam team)
    {
        return [.. Utilities.GetPlayers().Where(p => p != null && p.IsValid && p.Team == team)];
    }
}