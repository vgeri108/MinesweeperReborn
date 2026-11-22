using DiscordRPC;
using MinesweeperReborn.Localization;

namespace MinesweeperReborn.Discord;

internal static class DiscordPresence
{
    private const string ApplicationId = "1573305875286724818";

    private static DiscordRpcClient? client;

    public static void Initialize()
    {
        if (client != null)
            return;

        try
        {
            client = new DiscordRpcClient(ApplicationId);
            client.Initialize();

            SetPlaying();
        }
        catch
        {
            client?.Dispose();
            client = null;
        }
    }

    public static void SetPlaying()
    {
        if (client == null)
            return;

        try
        {
            client.SetPresence(new RichPresence
            {
                Details = Strings.DiscordDetails,
                State = string.Format(Strings.DiscordState, GameDetails.Height, GameDetails.Width, GameDetails.MineCount),

                Assets = new Assets
                {
                    LargeImageKey = "minesweeper",
                    LargeImageText = Strings.DiscordLargeText
                }
            });
        }
        catch
        {
            
        }
    }

    public static void Shutdown()
    {
        client?.Dispose();
        client = null;
    }
}