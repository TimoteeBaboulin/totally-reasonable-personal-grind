using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class InventoryCommand : ICommand
{
    private class InventoryMessageBuilder(List<Inventory> inventories)
    {
        public void WriteInventoryMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Inventory");
            foreach (var current in inventories)
            {
                if (current.Quantity == 0) continue;
                Item item = ItemAccess.GetItemById(current.ItemId).Result;
                embed.AddField(new EmbedFieldBuilder().WithName("ㅤ").WithValue(item.Name + " " + Item.EmojiFromName(item)).WithIsInline(true));
                embed.AddField(new EmbedFieldBuilder().WithName("ㅤ").WithValue(item.SellValue + " :coin:").WithIsInline(true));
                embed.AddField(new EmbedFieldBuilder().WithName("ㅤ").WithValue("x" + current.Quantity).WithIsInline(true));
            }

            properties.Embed = embed.Build();
        }
    }

    public ulong Id { get; } = ICommand.Index++;

    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("inventory").WithDescription("Show current inventory");

        SlashCommandProperties properties = builder.Build();

        var task = BotProgram.CreateSlashCommand(properties).Result;

        return task;
    }


    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        //Get Player Inventory
        var inv = InventoryAccess.GetInventoryData(command.User.GlobalName).Result;
        InventoryMessageBuilder builder = new(inv);

        // Display Inventory
        command.ModifyOriginalResponseAsync(builder.WriteInventoryMessage);

        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        throw new NotImplementedException();
    }
}