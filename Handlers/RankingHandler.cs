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
    private readonly ChatHelper chat;

    public RankingHandler(RankingDatabase db, MasterConfig config)
    {
        _db = db;
        _config = config;
        chat = new(config);
    }

    public HookResult OnPlayerConnectFullHandler(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event.Userid == null)
            return HookResult.Continue;

        var steamId = @event.Userid.SteamID;

        if (!_db.PlayerExists(steamId))
            _db.AddNewPlayer(@event.Userid.SteamID, @event.Userid.PlayerName);
        else
            _db.UpdatePlayerScore(steamId, 0, @event.Userid.PlayerName);

        return HookResult.Continue;
    }

    public HookResult OnPlayerDeathHandler(EventPlayerDeath @event, GameEventInfo info)
    {
        if (@event.Userid == null || @event.Attacker == null)
            return HookResult.Continue;

        var killer = @event.Attacker;
        var victim = @event.Userid;

        if (killer.SteamID == victim.SteamID)
            return HookResult.Continue;

        int expGained = _config.Ranking.ExpPerKill;
        if (@event.Headshot)
            expGained += _config.Ranking.ExpPerHeadshot;

        _db.UpdatePlayerScore(killer.SteamID, expGained, killer.PlayerName);

        var expLost = _config.Ranking.ExpLostPerDeath;
        _db.UpdatePlayerScore(victim.SteamID, -expLost, victim.PlayerName);

        if (_config.Messages.EnableMessages)
        {
            chat.PrintToChat(killer, _config.Messages.KillPlayerMessage,
                new() { { "exp", expGained.ToString() } });

            if (@event.Headshot)
            {
                chat.PrintToChat(killer, _config.Messages.HeadshotKillMessage,
                    new() { { "exp", _config.Ranking.ExpPerHeadshot.ToString() } });
            }

            chat.PrintToChat(victim, _config.Messages.DeathMessage,
                new() { { "exp", expLost.ToString() } });
        }

        return HookResult.Continue;
    }

    public HookResult OnBombDefusedHandler(EventBombDefused @event, GameEventInfo info)
    {
        var defuser = @event.Userid;
        if (defuser == null)
            return HookResult.Continue;

        var expGained = _config.Ranking.ExpPerBombDefuse;
        _db.UpdatePlayerScore(defuser.SteamID, expGained, defuser.PlayerName);

        if (_config.Messages.EnableMessages)
        {
            chat.PrintToChat(defuser, _config.Messages.BombDefuseMessage,
                new() { { "exp", expGained.ToString() } });
        }

        return HookResult.Continue;
    }

    public HookResult OnBombPlantedHandler(EventBombPlanted @event, GameEventInfo info)
    {
        var planter = @event.Userid;
        if (planter == null)
            return HookResult.Continue;

        var expGained = _config.Ranking.ExpPerBombPlant;
        _db.UpdatePlayerScore(planter.SteamID, expGained, planter.PlayerName);

        if (_config.Messages.EnableMessages)
        {
            chat.PrintToChat(planter, _config.Messages.BombPlantMessage,
                new() { { "exp", expGained.ToString() } });
        }

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
            if (_config.Messages.EnableMessages)
            {
                chat.PrintToChat(winner, _config.Messages.RoundWinMessage,
                    new() { { "exp", expGain.ToString() } });
            }
        });

        losers.ForEach((loser) =>
        {
            _db.UpdatePlayerScore(loser.SteamID, -expLoss, loser.PlayerName);
            if (_config.Messages.EnableMessages)
            {
                chat.PrintToChat(loser, _config.Messages.RoundLossMessage,
                    new() { { "exp", expLoss.ToString() } });
            }
        });

        return HookResult.Continue;
    }
}
