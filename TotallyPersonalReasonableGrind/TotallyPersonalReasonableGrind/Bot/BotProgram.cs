using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using HttpClient = TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access.HttpClient;

namespace TotallyPersonalReasonableGrind.Bot;

public class BotProgram
{
    private static DiscordSocketClient m_client = new();
    private CommandDispatcher.CommandDispatcher m_commandDispatcher = new();

    public static async Task<SocketApplicationCommand> CreateSlashCommand(SlashCommandProperties properties)
    {
        return await m_client.CreateGlobalApplicationCommandAsync(properties);
    }
    
    public async Task OnClientReady()
    {
        // Register commands when the client is ready
        m_commandDispatcher.RegisterCommands();
    }
    
    public async void Main()
    {
        HttpClient.Start();
        
        m_client.Ready += OnClientReady;
        m_client.SlashCommandExecuted += m_commandDispatcher.OnSlashCommandExecuted;
        m_client.ButtonExecuted += m_commandDispatcher.OnComponentExecuted;
        m_client.SelectMenuExecuted += m_commandDispatcher.OnComponentExecuted;
        m_client.Log += async (LogMessage msg) =>
        {
            Console.WriteLine(msg.ToString());
        };
        
        await m_client.LoginAsync(TokenType.Bot, "");
        await m_client.StartAsync();
    }
}