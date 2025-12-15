using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands;

public class Game : ICommand
{
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("play").WithDescription("Start a game");
        
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        task.Wait();
        var result = task.Result;
        
        return result;
    }
    
    public ulong Id { get; }
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        ComponentBuilder builder = new();
        builder.WithButton("Move", Id + "|move");
        builder.WithButton("Walk", Id + "|walk");
        builder.WithButton("Sell", Id + "|sell");
        builder.WithButton("Quit", Id + "|quit");
        
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You're actually in the game!");
        command.RespondAsync(embed: embed.Build(), components: builder.Build());
        
        return Task.FromResult(true);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        string command = component.Data.CustomId.Split('|')[1];
        switch (command)
        {
            case "move":
                component.RespondAsync("You moved!");
                break;
            case "walk":
                component.RespondAsync("You walked!");
                break;
            case "sell":
                component.RespondAsync("You sold!");
                break;
            case "quit":
                component.RespondAsync("You quit!");
                return Task.FromResult(false);
        }
        
        return Task.FromResult(true);
    }
}