using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class PlayerInfo : ICommand
{
    public ulong Id { get; }

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

    

    
    public Task<bool> OnSlashCommand(SocketSlashCommand command) {
        // Display Player Info
        EmbedBuilder embed = new();
        embed.WithTitle("Player Info");
        embed.WithAuthor(command.User.GlobalName);
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat Xp").WithValue("test"));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat LvL").WithValue("test"));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration Xp").WithValue("test"));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration LvL").WithValue("test"));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Current Position").WithValue("test"));
        command.RespondAsync(embed: embed.Build());
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        return Task.FromResult(false);
    }
}