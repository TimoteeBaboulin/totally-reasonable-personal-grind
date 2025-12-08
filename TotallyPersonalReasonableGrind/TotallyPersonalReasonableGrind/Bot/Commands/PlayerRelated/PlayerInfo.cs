using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class PlayerInfo : ICommand
{
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("playerinfo").WithDescription("Get info about you");
        
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        task.Wait();
        var result = task.Result;
        
        return result;
    }

    public ulong Id { get; }
    

    
    public Task<bool> OnSlashCommand(SocketSlashCommand command) {
        // Display Player Info
        command.RespondAsync("Player Info");
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        return Task.FromResult(false);
    }
}