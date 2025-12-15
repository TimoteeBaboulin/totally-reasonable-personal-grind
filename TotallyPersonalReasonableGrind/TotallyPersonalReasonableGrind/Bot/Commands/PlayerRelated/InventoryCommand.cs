using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class InventoryCommand : ICommand
{
    public ulong Id { get; } = ICommand.Index++; 
    
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("inventory").WithDescription("Show current inventory");
        
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        task.Wait();
        var result = task.Result;
        
        return result;
    }

    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        // Display Inventory
        EmbedBuilder embed = new();
        embed.WithTitle("Inventory");
        embed.WithAuthor(command.User.GlobalName);
        for (int i = 0; i < 1; i++)
        {
            embed.AddField(new EmbedFieldBuilder().WithValue("ItemName").WithIsInline(true));
            embed.AddField(new EmbedFieldBuilder().WithValue("ItemGold").WithIsInline(true));
            embed.AddField(new EmbedFieldBuilder().WithValue("ItemNumber").WithIsInline(true));
            embed.AddField(new EmbedFieldBuilder());
        }
        command.RespondAsync(embed: embed.Build());
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}