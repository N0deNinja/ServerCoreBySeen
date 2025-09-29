using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using ServerCoreBySeen.Configs;
using ServerCoreBySeen.Helpers;

public class PlayerHandler
{
    private MasterConfig _config = null!;

    public PlayerHandler(MasterConfig config)
    {
        _config = config;
    }

    public HookResult OnPlayerConnectFull(EventPlayerConnectFull Player, GameEventInfo gameInfo)
    {
        var player = Player.Userid;
        if (player == null)
            return HookResult.Continue;

        if (_config.General.EnableTags)
        {
            if (GameHelper.IsPlayerVip(player.SteamID, _config))
            {
                player.Clan = _config.Scoreboard.VipTag;
            }

            if (GameHelper.IsPlayerAdmin(player.SteamID, _config))
            {
                player.Clan = _config.Scoreboard.AdminTag; ;
            }
        }

        return HookResult.Continue;
    }

    public HookResult OnRoundStartHandler(EventRoundStart @event, GameEventInfo gameInfo)
    {
        var allPlayers = GameHelper.GetAllPlayersInTeams();
        allPlayers.ForEach((player) =>
        {
            PlayerHelper.ExecuteVipBenefits(player, _config);
        });
        return HookResult.Continue;
    }
}