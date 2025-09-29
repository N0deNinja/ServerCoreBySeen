
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using ServerCoreBySeen.Configs;

namespace ServerCoreBySeen.Helpers;

public class PlayerHelper
{
    public static void ExecuteVipBenefits(CCSPlayerController Player, MasterConfig _config)
    {
        if (!GameHelper.IsPlayerVip(Player.SteamID, _config))
            return;

        foreach (var item in _config.Vip.RoundStartItems)
        {


            switch (item)
            {
                case "fire_grenade":
                    if (Player.Team == CsTeam.Terrorist)
                        Player.GiveNamedItem("weapon_molotov");
                    else if (Player.Team == CsTeam.CounterTerrorist)
                        Player.GiveNamedItem("weapon_incgrenade");
                    break;

                default:
                    Player.GiveNamedItem(item);
                    break;
            }
        }
    }

}