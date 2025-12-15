using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Helpers;
using TotallyPersonalReasonableGrind.Bot.Interfaces;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.Commands;

public class Game : ICommand
{
    public static SocketApplicationCommand BuildProperties()
    {
        SlashCommandBuilder builder = new();
        builder.WithName("play").WithDescription("Start a game");
        
        SlashCommandProperties properties = builder.Build();
        
        var task = BotProgram.CreateSlashCommand(properties);
        var result = task.Result;

        return result;
    }

    public ulong Id { get; } = ++ICommand.Index;

    // Action Map--------------------------------------------------------------------------------
    private static Dictionary<string, Func<SocketMessageComponent, bool>>? m_actionMap = null;
    
    private void PopulateActionMap()
    {
        m_actionMap = new Dictionary<string, Func<SocketMessageComponent, bool>>
        {
            { "move"            , OnMove         },
            { "walk"            , OnWalk         },
            { "sell"            , OnSell         },
            { "hit"             , OnHit          },
            { "shop"            , OnShop         },
            { "quit"            , OnQuit         },
            { "sell_item"       , OnSellItem     },
            { "sell_quantity"   , OnSellQuantity },
            { "confirm_sell"    , OnConfirmSell  },
            { "back"            , OnSellBack     }
        };
    }
    
    MessageComponent Components
    {
        get
        {
            ComponentBuilder builder = new();
            builder.WithButton("Walk", Id + "|walk");
            builder.WithButton("Hit" , Id + "|hit" );
            builder.WithButton("Sell", Id + "|sell");
            builder.WithButton("Shop", Id + "|shop");
            builder.WithButton("Move", Id + "|move");
            builder.WithButton("Quit", Id + "|quit");
            return builder.Build();
        }
    }
    
    private void WriteInitialMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("Welcome to the game!");
        embed.WithColor(Color.Blue);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("initial");
        
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    // Player Data-------------------------------------------------------------------------------
    private Player m_player;
    private Item m_currentItem;
    private int m_currentQuantity;
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        command.DeferAsync();

        Task<Player> task = PlayerAccess.GetOrCreatePlayer(command.User.GlobalName);
        
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You're actually in the game!");
        command.ModifyOriginalResponseAsync(WriteInitialMessage);
        
        m_player = task.Result;
        
        return Task.FromResult(true);
    }

    // LOOT---------------------------------------------------------------------------------------
    
    private class LootMessageBuilder
    {
        public Loot Loot { get; set; }

        public LootMessageBuilder(Loot loot)
        {
            Loot = loot;
        }

        Item GetItemFromLoot(Loot loot)
        {
            int itemId = loot.ItemId;
            return ItemAccess.GetItemById(itemId).Result;
        }
        
        string EmojiString(string emojiName)
        {
            return ":" + emojiName + ":";
        }
        
        public void WriteLootMessage(MessageProperties properties)
        {
            Item item = GetItemFromLoot(Loot);
            List<EmbedFieldBuilder> fields = new();
            fields.Add(new EmbedFieldBuilder().WithName("Item").WithValue(Loot.Quantity + " " + item.Name + " " + EmojiString(item.EmojiName)).WithIsInline(true));
            
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            embed.WithDescription("You got loot!");
            embed.WithColor(Color.Purple);
            embed.ImageUrl = ImageLinkHelper.GetImageLink("walk");

            embed.Fields = fields;
            
            properties.Embed = embed.Build();
        }
    }
    
    Loot[] GetLootFromArea(LootType _itemType)
    {
        List<Loot> rawLoots = LootAccess.GetLootFromArea(m_player.AreaId).Result;
        return rawLoots.Where((obj) => obj.Type == _itemType).ToArray();
    }
    
    void AddLootToPlayerInventory(Loot loot, int quantity)
    {
        Item item = ItemAccess.GetItemById(loot.ItemId).Result;
        InventoryAccess.AddItemToInventory(m_player.Name, item.Name, quantity).Wait();
    }
    
    Loot GetRandomLoot(LootType _itemType)
    {
        Loot[] loots = GetLootFromArea(_itemType);
        int totalWeight = 0;
        foreach (var loot in loots)
        {
            switch (loot.Rarity)
            {
                case LootRarity.Legendary:
                    totalWeight += 10;
                    break;
                case LootRarity.Epic:
                    totalWeight += 5;
                    break;
                case LootRarity.Rare:
                    totalWeight += 3;
                    break;
                case LootRarity.UnCommon:
                    totalWeight += 2;
                    break;
                case LootRarity.Common:
                    totalWeight += 1;
                    break;
                default:
                    break;
            }
        }
        
        int randomValue = new Random().Next(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (var loot in loots)
        {
            int weight = loot.Rarity switch
            {
                LootRarity.Legendary => 10,
                LootRarity.Epic => 5,
                LootRarity.Rare => 3,
                LootRarity.UnCommon => 2,
                LootRarity.Common => 1,
                _ => 0
            };
            
            cumulativeWeight += weight;
            if (randomValue < cumulativeWeight)
            {
                return loot;
            }
        }
        
        throw new InvalidDataException("Failed to get random loot.");
    }
    
    // WALK--------------------------------------------------------------------------------------
    private bool OnWalk(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        Loot loot = GetRandomLoot(LootType.Walk);
        LootMessageBuilder lootMessageBuilder = new(loot);
        AddLootToPlayerInventory(loot, loot.Quantity);
        
        msg.ModifyAsync(lootMessageBuilder.WriteLootMessage).Wait();
        return true;
    }
    
    // SELL---------------------------------------------------------------------------------------

    private class SellItemMessageBuilder
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public bool SaleSuccessful { get; set; }

        public SellItemMessageBuilder(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
        
        public void WriteSellItemMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            if (SaleSuccessful)
                embed.WithDescription("You sold " + Quantity +" "+ Item.Name + "!");
            else
                embed.WithDescription("You couldn't sell " + Quantity +" "+ Item.Name + "!");
            embed.WithColor(Color.Gold);
            embed.ImageUrl = ImageLinkHelper.GetImageLink("sell");
            
            properties.Embed = embed.Build();
        }
    }
    
    bool PlayerHasItem(Item item, int quantity)
    {
        Inventory[] inventories = GetPlayerInventory();
        foreach (var inventory in inventories)
        {
            if (inventory.ItemId != item.Id)
            {
                continue;
            }
            
            if (inventory.Quantity >= quantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        throw new NotImplementedException();
    }
    
    void RemovePlayerItem(Item item, int quantity)
    {
        throw new NotImplementedException();
    }
    
    void AddPlayerGold(int amount)
    {
        throw new NotImplementedException();
    }
    
    bool SellItem(Item item, int quantity, SocketMessageComponent component)
    {
        int sellValue = item.SellValue * quantity;
        SellItemMessageBuilder sellItemMessageBuilder = new(item, sellValue);
        
        if (!PlayerHasItem(item, quantity))
        {
            sellItemMessageBuilder.SaleSuccessful = false;
        }
        else
        {
            AddPlayerGold(sellValue);
            RemovePlayerItem(item, quantity);
            sellItemMessageBuilder.SaleSuccessful = true;
        }
        
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(sellItemMessageBuilder.WriteSellItemMessage).Wait();
        return true;
    }
    
    Inventory[] GetPlayerInventory()
    {
        return InventoryAccess.GetInventoryData(m_player.Name).Result.ToArray();
    }
    
    MessageComponent GetInventoryComponents(Inventory[] inventories)
    {
        SelectMenuBuilder selectMenuBuilder = new();
        selectMenuBuilder.WithCustomId(Id + "|sell_item")
            .WithPlaceholder("Select an item to sell")
            .WithType(ComponentType.SelectMenu);
        foreach (var inventory in inventories)
        {
            Item item = ItemAccess.GetItemById(inventory.ItemId).Result;
            selectMenuBuilder.AddOption(item.Name + " x" + inventory.Quantity, inventory.ItemId.ToString());
        }
        
        TextInputBuilder textInputBuilder = new();
        textInputBuilder.WithCustomId(Id + "|sell_quantity")
            .WithLabel("Quantity to sell")
            .WithStyle(TextInputStyle.Short)
            .WithPlaceholder("Enter quantity")
            .WithRequired(true);
        
        ButtonBuilder buttonBuilder = new();
        buttonBuilder.WithLabel("Sell Item")
            .WithCustomId(Id + "|confirm_sell")
            .WithStyle(ButtonStyle.Danger);
        
        ButtonBuilder backButtonBuilder = new();
        backButtonBuilder.WithLabel("Back")
            .WithCustomId(Id + "|back")
            .WithStyle(ButtonStyle.Secondary);
        
        ActionRowBuilder actionRowBuilder = new();
        actionRowBuilder.AddComponent(buttonBuilder);
        actionRowBuilder.AddComponent(backButtonBuilder);
        actionRowBuilder.AddComponent(selectMenuBuilder);
        actionRowBuilder.AddComponent(textInputBuilder);
        
        ComponentBuilderV2 builderV2 = new();
        builderV2.WithActionRow(actionRowBuilder);
        return builderV2.Build();
    }
    
    private void WriteSellMessage(MessageProperties properties)
    {
        Inventory[] inventories = GetPlayerInventory();
        
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You sold an item!");
        embed.WithColor(Color.Gold);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("sell");
        
        properties.Embed = embed.Build();
        properties.Components = GetInventoryComponents(inventories);
    }
    
    private bool OnSell(SocketMessageComponent component)
    {
        //SellItem();
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteSellMessage).Wait();
        return true;
    }
    
    private bool OnSellItem(SocketMessageComponent component)
    {
        int itemId = Int32.Parse(component.Data.Values.First());
        Item item = ItemAccess.GetItemById(itemId).Result;
        
        m_currentItem = item;
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteSellMessage);
        return true;
    }
    
    private bool OnSellQuantity(SocketMessageComponent component)
    {
        int quantity;
        bool parseResult = int.TryParse(component.Data.Values.First(), out quantity);
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        
        if (!parseResult || quantity <= 0)
        {
            component.RespondAsync("Invalid quantity.").Wait();
            return true;
        }
        
        m_currentQuantity = quantity;
        msg.ModifyAsync(WriteSellMessage);
        return true;
    }
    
    private bool OnConfirmSell(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        return SellItem(m_currentItem, m_currentQuantity, component);
    }
    
    private bool OnSellBack(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteInitialMessage).Wait();
        return true;
    }
    
    // HIT----------------------------------------------------------------------------------------
    
    private bool OnHit(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        Loot loot = GetRandomLoot(LootType.Hit);
        LootMessageBuilder lootMessageBuilder = new(loot);
        
        msg.ModifyAsync(lootMessageBuilder.WriteLootMessage).Wait();
        return true;
    }
    
    // SHOP---------------------------------------------------------------------------------------
    
    private void WriteShopMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("Welcome to the shop!");
        embed.WithColor(Color.Green);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("shop");
            
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    private bool OnShop(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteShopMessage).Wait();
        return true;
    }
    
    // MOVE---------------------------------------------------------------------------------------
    Area[] GetAreas()
    {
        throw new NotImplementedException();
    }
    
    private void WriteMoveMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You moved to a new location!");
        embed.WithColor(Color.Red);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("move");
        
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    private bool OnMove(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteMoveMessage).Wait();
        return true;
    }
    
    // QUIT---------------------------------------------------------------------------------------
    
    private bool OnQuit(SocketMessageComponent component)
    {
        throw new System.NotImplementedException();
    }
    
    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        string command = component.Data.CustomId.Split('|')[1];
        
        // This should never be needed since it should be populated at construction
        if (m_actionMap == null)
        {
            PopulateActionMap();
        }
        
        if (m_actionMap.TryGetValue(command, out var action))
        {
            return Task.FromResult(action(component));
        }
        
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(properties =>
        {
            properties.Content = "Unknown action.";
            properties.Components = Components;
        }).Wait();
        
        return Task.FromResult(true);
    }
}