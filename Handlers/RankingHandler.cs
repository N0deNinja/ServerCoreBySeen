using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using ServerCoreBySeen.Configs;
using ServerCoreBySeen.Database;
using ServerCoreBySeen.Helpers;

namespace ServerCoreBySeen.Handlers;

public class RankingHandler
{

    private readonly RankingDatabase _db;
    private readonly MasterConfig _config;

    public RankingHandler(RankingDatabase db, MasterConfig config)
    {
        _db = db;
        _config = config;
    }

    public HookResult OnPlayerConnectFullHandler(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event.Userid == null)
            return HookResult.Continue;

        var steamId = @event.Userid.SteamID;

        if (_config.General.EnableRanking)
        {
            if (!_db.PlayerExists(steamId))
                _db.AddNewPlayer(@event.Userid.SteamID, @event.Userid.PlayerName);
            else
                _db.UpdatePlayerScore(steamId, 0, @event.Userid.PlayerName);
        }

        return HookResult.Continue;
    }

    public HookResult OnPlayerDeathHandler(EventPlayerDeath @event, GameEventInfo info)
    {
        if (@event.Userid == null || @event.Attacker == null)
            return HookResult.Continue;

        var killerId = @event.Attacker.SteamID;
        var victimId = @event.Userid.SteamID;

        if (killerId == victimId)
            return HookResult.Continue;

        if (_config.General.EnableRanking)
        {
            int expGained = _config.Ranking.ExpPerKill;
            if (@event.Headshot)
                expGained += _config.Ranking.ExpPerHeadshot;

            _db.UpdatePlayerScore(killerId, expGained, @event.Attacker.PlayerName);


            var expLost = _config.Ranking.ExpLostPerDeath;


            _db.UpdatePlayerScore(victimId, -expLost, @event.Userid.PlayerName);

        }

        return HookResult.Continue;
    }

    public HookResult OnBombDefusedHandler(EventBombDefused @event, GameEventInfo info)
    {
        var defuser = @event.Userid;

        if (defuser == null)
        {
            return HookResult.Continue;
        }

        var expGained = _config.Ranking.ExpPerBombDefuse;

        _db.UpdatePlayerScore(defuser.SteamID, expGained, defuser.PlayerName);

        return HookResult.Continue;
    }

    public HookResult OnRoundEndHandler(EventRoundEnd @event, GameEventInfo info)
    {
        var winnerTeam = (CsTeam)@event.Winner;
        var loserTeam = winnerTeam == CsTeam.CounterTerrorist ? CsTeam.Terrorist : CsTeam.CounterTerrorist;

        var winners = GameHelper.GetPlayersByTeam(winnerTeam);
        var losers = GameHelper.GetPlayersByTeam(loserTeam);

        var expGain = _config.Ranking.ExpPerRoundWin;
        var expLoss = _config.Ranking.ExpLostPerRoundLoss;

        winners.ForEach((winner) =>
        {
            _db.UpdatePlayerScore(winner.SteamID, expGain, winner.PlayerName);
        });

        losers.ForEach((loser) =>
        {
            _db.UpdatePlayerScore(loser.SteamID, -expLoss, loser.PlayerName);
        });

        return HookResult.Continue;
    }

    public HookResult OnBombPlantedHandler(EventBombPlanted @event, GameEventInfo info)
    {
        if (@event.Userid == null)
        {
            return HookResult.Continue;
        }

        var planter = @event.Userid;
        var expGained = _config.Ranking.ExpPerBombPlant;

        _db.UpdatePlayerScore(planter.SteamID, expGained, planter.PlayerName);
        return HookResult.Continue;
    }
}