using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using ServerCoreBySeen.Configs;

public class ChatHelper
{
    private static readonly Dictionary<string, string> colorMap = new()
    {
        { "default", "\x01" },
        { "red",     "\x02" },
        { "purple",  "\x03" },
        { "green",   "\x04" },
        { "lightgreen", "\x05" },
        { "lime",    "\x06" },
        { "grey",    "\x08" },
        { "lightpurple", "\x0E" },
        { "lightred", "\x0F" },
        { "gold",    "\x10" }
    };

    private readonly MasterConfig _config;

    public ChatHelper(MasterConfig config)
    {
        _config = config;
    }

    private static string ApplyColors(string message)
    {
        foreach (var kv in colorMap)
        {
            message = message.Replace("{" + kv.Key + "}", kv.Value);
        }

        return message + colorMap["default"];
    }

    private static string ReplaceVariables(string message, Dictionary<string, string>? variables)
    {
        if (variables == null) return message;

        foreach (var kv in variables)
        {
            message = message.Replace("{" + kv.Key + "}", kv.Value);
        }
        return message;
    }

    private static string PrepareMessage(string message, Dictionary<string, string>? variables = null)
    {
        var withVars = ReplaceVariables(message, variables);
        return ApplyColors(withVars);
    }

    public void PrintToChat(CCSPlayerController player, string message, Dictionary<string, string>? variables = null)
    {
        if (_config.General.EnableMessages)
        {
            player.PrintToChat(_config.Messages.Prefix + " " + PrepareMessage(message, variables));
        }
    }

    public void PrintToAll(string message, Dictionary<string, string>? variables = null)
    {
        if (_config.General.EnableMessages)
        {
            Server.PrintToChatAll(_config.Messages.Prefix + " " + PrepareMessage(message, variables));
        }
    }
}
