using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class LootInfoCommand : ICommand
{
    private class LootInfoMessageBuilder(List<Loot> loots, Item item)
    {
        public void WriteLootMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle(item.Name + " " + Item.EmojiFromName(item));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Area").WithValue(AreaAccess.GetAreaById(loots[0].AreaId).Result.Name).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Rarity").WithValue(loots[0].Rarity.ToString()).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Required Level").WithValue(loots[0].RequiredLevel).WithIsInline(true));
            loots.RemoveAt(0);
            foreach (var loot in loots)
            {
                embed.Fields.Add(new EmbedFieldBuilder().WithName("-").WithValue(AreaAccess.GetAreaById(loot.AreaId).Result.Name).WithIsInline(true));
                embed.Fields.Add(new EmbedFieldBuilder().WithName("-").WithValue(loot.Rarity.ToString()).WithIsInline(true));
                embed.Fields.Add(new EmbedFieldBuilder().WithName("-").WithValue(loot.RequiredLevel).WithIsInline(true));
            }

            properties.Embed = embed.Build();
        }
    }
    public ulong Id { get; }
    
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandOptionBuilder list = new();
        list.WithName("item").WithDescription("Item to get info about").WithType(ApplicationCommandOptionType.String).WithRequired(true);
        var itemList = ItemAccess.GetAllItems().Result;
        for (var index = 0; index < itemList.Count; index++)
        {
            var lootName = itemList[index];
            list.AddChoice(lootName.Name, lootName.Name);
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
        List<Loot> rightLoot = LootAccess.GetLootEntriesByItemName((string)itemName).Result;
        LootInfoMessageBuilder builder = new(rightLoot, ItemAccess.GetItemById(rightLoot[0].ItemId).Result);
        
        //Display loot Info
        command.ModifyOriginalResponseAsync(builder.WriteLootMessage);
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}