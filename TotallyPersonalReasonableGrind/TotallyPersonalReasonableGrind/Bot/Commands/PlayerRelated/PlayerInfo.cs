using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class PlayerInfo : ICommand
{
    public static SocketApplicationCommand BuildProperties()
    {
        //Create Command
        SlashCommandBuilder builder = new();
        builder.WithName("playerinfo").WithDescription("Get info about you");
        
        SlashCommandProperties properties = builder.Build();
        
        // Register the Command
        var slashCommand = BotProgram.CreateSlashCommand(properties);
        slashCommand.Wait();
        var result = slashCommand.Result;
        
        Console.WriteLine("passed here");
        
        return result;
    }

    public ulong Id { get; }
    

    
    public Task<bool> OnSlashCommand(SocketSlashCommand command) {
        // Display Player Info
        ComponentBuilder builder = new();
        builder.WithButton("playerButton", Id + "|playerButton");
        
        command.RespondAsync("Player Info", components: builder.Build());
        
        return Task.FromResult(true);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        component.RespondAsync("Player Info");
        return Task.FromResult(false);
    }
}