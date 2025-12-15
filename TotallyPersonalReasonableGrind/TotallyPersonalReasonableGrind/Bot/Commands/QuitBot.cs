using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands;

public class QuitBot : ICommand
{
    public ulong Id { get; }
    
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("quitbot").WithDescription("Quit the bot");
        
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        task.Wait();
        var result = task.Result;
        
        return result;
    }

    
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        BotProgram.m_client.StopAsync().Wait();
        BotProgram.m_client.LogoutAsync().Wait();
        BotProgram.m_client.Dispose();
        Console.WriteLine("Bot stopped");
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}