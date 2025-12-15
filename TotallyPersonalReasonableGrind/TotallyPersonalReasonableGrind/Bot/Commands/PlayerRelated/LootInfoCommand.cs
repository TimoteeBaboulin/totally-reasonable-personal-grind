using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class LootInfoCommand : ICommand
{
    public ulong Id { get; }
    
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandOptionBuilder list = new();
        list.WithName("item").WithDescription("Item to get info about").WithType(ApplicationCommandOptionType.String).WithRequired(true);
        string[] oui = ["test", "try" , "worth"];
        for (var index = 0; index < oui.Length; index++)
        {
            var lootName = oui[index];
            list.AddChoice(lootName, lootName);
        }

        SlashCommandBuilder builder = new();
        builder.WithName("lootinfo").WithDescription("Show info of an item").AddOption(list);
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        task.Wait();
        var result = task.Result;
        
        return result;
    }
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        //Get Item Info
        var itemName = command.Data.Options.First().Value;
        
        
        //Display loot Info
        EmbedBuilder embed = new();
        embed.WithTitle((string)itemName);
        embed.WithAuthor(command.User.GlobalName);
        embed.ImageUrl = "https://i.imgur.com/2521111.png";
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Area").WithValue("Home").WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Rarity").WithValue("Golden").WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Required Level").WithValue("Too much").WithIsInline(true));
        command.RespondAsync(embed: embed.Build());
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}