using System;
using System.IO;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Dapper;
using Microsoft.Data.Sqlite;
using ServerCoreBySeen.Configs;
using ServerCoreBySeen.Helpers;

namespace ServerCoreBySeen.Database
{
    public class RankingDatabase
    {
        private SqliteConnection? _connection;
        private readonly MasterConfig _config = null!;

        public RankingDatabase(MasterConfig config)
        {
            _config = config;
        }

        public void Initialize(string directory)
        {
            var dbPath = Path.Join(directory, "ranking.db");
            _connection = new SqliteConnection($"Data Source={dbPath}");
            _connection.Open();

            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS players (
                    steamid   UNSIGNED BIG INT NOT NULL PRIMARY KEY,
                    name      VARCHAR(64),
                    score     INT NOT NULL DEFAULT 0
                );
            ");
        }

        public bool PlayerExists(ulong steamId)
        {
            return _connection!.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM players WHERE steamid = @steamId",
                new { steamId }) > 0;
        }

        public void AddNewPlayer(ulong SteamID, string PlayerName)
        {
            Console.WriteLine($"[RankingDB] Adding player {PlayerName} ({SteamID})");

            _connection!.Execute(@"
                INSERT INTO players (steamid, name, score)
                VALUES (@steamId, @name, 0);",
                new { steamId = SteamID, name = PlayerName });
        }

        public DatabasePlayer GetPlayer(ulong steamId)
        {
            return _connection!.QueryFirstOrDefault<DatabasePlayer>(
                "SELECT * FROM players WHERE steamid = @steamId",
                new { steamId })
                ?? new DatabasePlayer { SteamId = steamId, Name = string.Empty, Score = 0 };
        }

        public void UpdatePlayerScore(ulong steamId, int deltaScore, string name = "")
        {
            if (!PlayerExists(steamId))
            {
                Console.WriteLine($"[RankingDB] Player {steamId} not in DB, adding...");
                _connection!.Execute(@"
            INSERT INTO players (steamid, name, score)
            VALUES (@steamId, @name, @deltaScore);",
                    new { steamId, name, deltaScore });
                return;
            }

            var extraExp = _config.Vip.ExtraExp;

            if (deltaScore < 0)
            {
                extraExp = 0;
                var player = GetPlayer(steamId);
                if (player.Score + deltaScore < 0)
                    return;
            }

            if (GameHelper.IsPlayerVip(steamId, _config))
            {
                deltaScore += extraExp;
            }

            _connection!.Execute(@"
                UPDATE players
                SET score = score + @deltaScore,
                    name = CASE WHEN @name <> '' THEN @name ELSE name END
                WHERE steamid = @steamId;",
             new { steamId, deltaScore, name });
        }


        public IEnumerable<DatabasePlayer> GetTopPlayers(int limit = 15)
        {
            return _connection!.Query<DatabasePlayer>(
                "SELECT * FROM players ORDER BY score DESC LIMIT @limit;",
                new { limit });
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _connection = null;
        }
    }

    public class DatabasePlayer
    {
        public ulong SteamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}
