using Discord;
using Discord.WebSocket;

namespace TotallyPersonalReasonableGrind.Bot;

public class BotProgram
{
    private DiscordSocketClient m_client = new();
    
    private async Task ClientReady()
    {
    }
    
    public async void Main()
    {
         m_client.Ready += ClientReady;
        
        await m_client.LoginAsync(Discord.TokenType.Bot);
        
        await m_client.StartAsync();


        await m_client.StartAsync();
    }
}