using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.MapRelated;

public class Move : ICommand
{
    Move() {
        //Create Command
        SlashCommandBuilder builder = new();
        
        // Retrieve possible maps from database and add them as option.
        SlashCommandOptionBuilder mapOption = new SlashCommandOptionBuilder()
            .WithName("map")
            .WithDescription("The map you wanna move to")
            .WithRequired(true);
        for (int i = 0; i < 1; i++)
        {
            mapOption.AddChoice("MapName", "MapID");
        }
        
        builder
            .WithName("move")
            .WithDescription("Change Map")
            .AddOption(mapOption);
        
        SlashCommandProperties properties = builder.Build();
        
        // Register the Command
        //SocketSlashCommand slashCommand = await 
        // RegisterCommand(slashCommand);
        // Id = slashCommand.Id;
    }

    public static SocketApplicationCommand BuildProperties()
    {
        return null;
    }

    public ulong Id { get; }
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}