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
    private Dictionary<string, Func<SocketMessageComponent, bool>>? m_actionMap = null;
    
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
            { "back"            , OnSellBack     },
            { "backMove"        , OnMoveBack     },
            { "backShop"        , OnShopBack     },
            { "buy_xp"          , OnShopBuyXp    },
            { "move_to"         , OnMoveChoose   }
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
    private IMessageChannel m_channel;
    private Item? m_currentItem = null;
    private int m_currentQuantity;
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        Task<Player> task = PlayerAccess.GetOrCreatePlayer(command.User.GlobalName);
        
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You're actually in the game!");
        command.ModifyOriginalResponseAsync(WriteInitialMessage);
        
        m_player = task.Result;
        m_channel = command.Channel;
        
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
        
        public void WriteLootMessage(MessageProperties properties)
        {
            Item item = GetItemFromLoot(Loot);
            List<EmbedFieldBuilder> fields = new();
            fields.Add(new EmbedFieldBuilder().WithName("Item").WithValue(Loot.Quantity + " " + item.Name + " " + Item.EmojiFromName(item)).WithIsInline(true));
            
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            embed.WithDescription("You got loot!");
            embed.WithColor(Loot.Rarity switch
            {
                LootRarity.Common => Color.LightGrey,
                LootRarity.UnCommon => Color.Green,
                LootRarity.Rare => Color.Blue,
                LootRarity.Epic => Color.Purple,
                LootRarity.Legendary => Color.Orange,
                _ => Color.DarkGrey
            });
            embed.ImageUrl = ImageLinkHelper.GetImageLink(Loot.Type == LootType.Walk ? "walk" : "hit");

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
        bool leveledUp;
        switch (loot.Type)
        {
            case LootType.Walk:
                leveledUp = PlayerAccess.UpdatePlayerExplorationStats(m_player.Name, quantity).Result;
                break;
            case LootType.Hit:
                leveledUp = PlayerAccess.UpdatePlayerCombatStats(m_player.Name, quantity).Result;
                break;
            default:
                throw new InvalidDataException("Invalid loot type.");
        }
        m_player = PlayerAccess.GetOrCreatePlayer(m_player.Name).Result;
        if (leveledUp)
        {
            int newLevel = loot.Type == LootType.Walk ? m_player.ExplorationLvl : m_player.CombatLvl;
            string levelType = loot.Type == LootType.Walk ? "Exploration" : "Combat";
            m_channel.SendMessageAsync("Congratulations " + m_player.Name + ", you leveled up your" + levelType+ " level to " + newLevel + "!");
        }
    }
    
    Loot GetRandomLoot(LootType _itemType)
    {
        Player player = PlayerAccess.GetOrCreatePlayer(m_player.Name).Result;
        int requiredLevel = _itemType == LootType.Walk ? player.ExplorationLvl : player.CombatLvl;
        Loot[] loots = GetLootFromArea(_itemType)
            .Where((loot) => loot.RequiredLevel <= requiredLevel)
            .ToArray();
        int totalWeight = 0;
        foreach (var loot in loots)
        {
            switch (loot.Rarity)
            {
                case LootRarity.Legendary:
                    totalWeight += 1;
                    break;
                case LootRarity.Epic:
                    totalWeight += 2;
                    break;
                case LootRarity.Rare:
                    totalWeight += 3;
                    break;
                case LootRarity.UnCommon:
                    totalWeight += 5;
                    break;
                case LootRarity.Common:
                    totalWeight += 10;
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
                LootRarity.Legendary => 1,
                LootRarity.Epic => 2,
                LootRarity.Rare => 3,
                LootRarity.UnCommon => 5,
                LootRarity.Common => 10,
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

    private class SellItemMessageBuilder(Item item, int quantity)
    {
        public bool SaleSuccessful { get; set; }
        
        public void WriteSellItemMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            if (SaleSuccessful)
                embed.WithDescription("You sold " + quantity +" "+ item.Name + "!");
            else
                embed.WithDescription("You couldn't sell " + quantity +" "+ item.Name + "!");
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
        InventoryAccess.RemoveItemFromInventory(m_player.Name, item.Name, quantity).Wait();
    }
    
    void AddPlayerGold(int amount)
    {
        PlayerAccess.UpdatePlayerMoney(m_player.Name, amount).Wait();
        m_player = PlayerAccess.GetOrCreatePlayer(m_player.Name).Result;
    }
    
    bool SellItem(Item item, int quantity, SocketMessageComponent component)
    {
        int sellValue = item.SellValue * quantity;
        SellItemMessageBuilder sellItemMessageBuilder = new(item, quantity);
        
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
        ComponentBuilder builder = new ComponentBuilder();
        
        SelectMenuBuilder selectMenuBuilder = new();
        selectMenuBuilder.WithCustomId(Id + "|sell_item")
            .WithPlaceholder("Select an item to sell")
            .WithType(ComponentType.SelectMenu);
        
        foreach (var inventory in inventories)
        {
            Item item = ItemAccess.GetItemById(inventory.ItemId).Result;
            selectMenuBuilder.AddOption(item.Name + " x" + inventory.Quantity, inventory.ItemId.ToString(), isDefault: item == m_currentItem);
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
        
        builder.WithSelectMenu(selectMenuBuilder)
            .WithButton(buttonBuilder)
            .WithButton(backButtonBuilder);
        
        if (false)
        {
            Inventory[] playerInventory = GetPlayerInventory();
            Inventory currentInventory = playerInventory.First((inv) => inv.ItemId == m_currentItem.Id);
            
            SelectMenuBuilder quantityMenuBuilder = new();
            quantityMenuBuilder.WithCustomId(Id + "|sell_quantity")
                .WithType(ComponentType.SelectMenu);
            for (int sellQuantity = 1; sellQuantity < currentInventory.Quantity; sellQuantity++)
            {
                quantityMenuBuilder.AddOption(sellQuantity.ToString(), sellQuantity.ToString(), isDefault: sellQuantity == m_currentQuantity);
            }
            
            builder.WithSelectMenu(quantityMenuBuilder);
        }
        
        return builder.Build();
    }
    
    private void WriteInitalSellMessage(MessageProperties properties)
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
        m_currentItem = null;
        m_currentQuantity = 1;
        
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteInitalSellMessage).Wait();
        return true;
    }
    
    private bool OnSellItem(SocketMessageComponent component)
    {
        int itemId = Int32.Parse(component.Data.Values.First());
        Item item = ItemAccess.GetItemById(itemId).Result;
        
        m_currentItem = item;
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteInitalSellMessage);
        return true;
    }
    
    private bool OnSellQuantity(SocketMessageComponent component)
    {
        int quantity = int.Parse(component.Data.Values.First());
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        
        if (quantity <= 0)
        {
            component.RespondAsync("Invalid quantity.").Wait();
            return true;
        }
        
        m_currentQuantity = quantity;
        msg.ModifyAsync(WriteInitalSellMessage);
        return true;
    }
    
    private bool OnConfirmSell(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        Inventory[] inventories = GetPlayerInventory();
        if (PlayerHasItem(m_currentItem, 1))
        {
            Inventory currentInventory = inventories.First((inv) => inv.ItemId == m_currentItem.Id);
            return SellItem(m_currentItem, currentInventory.Quantity, component);
        }
        
        component.RespondAsync("You don't have that item.").Wait();
        return true;
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
        AddLootToPlayerInventory(loot, loot.Quantity);
        
        msg.ModifyAsync(lootMessageBuilder.WriteLootMessage).Wait();
        return true;
    }
    
    // SHOP---------------------------------------------------------------------------------------

    private class ShopMessageBuilder(int xpValue, string xpType)
    {
        public int moneyValue { get; set; }
        public bool BuySuccessful { get; set; }
        
        public void WriteBuyMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            if (BuySuccessful)
                embed.WithDescription("You bought " + xpValue +" XP for "+ xpType + "!" + " You have " + moneyValue +" money.");
            else
                embed.WithDescription("You don't have enough money to buy " + xpValue +" XP for "+ xpType + "! You have " + moneyValue + " money.");
            embed.WithColor(Color.Green);
            embed.ImageUrl = ImageLinkHelper.GetImageLink("shop");
            
            properties.Embed = embed.Build();
        }
    }
    
    private void AddShopSelectMenuValues(SelectMenuBuilder selectMenu, int maxValue, string valueName)
    {
        int dividePerEight = (int)MathF.Round(maxValue / 8f);
        int dividePerFour = (int)MathF.Round(maxValue / 4f);
        int dividePerTwo = (int)MathF.Round(maxValue / 2f);
        selectMenu.AddOption($"{dividePerEight} {valueName} XP - {dividePerEight} money", $"{dividePerEight}|{valueName}");
        selectMenu.AddOption($"{dividePerFour} {valueName} XP - {dividePerFour} money", $"{dividePerFour}|{valueName}");
        selectMenu.AddOption($"{dividePerTwo} {valueName} XP - {dividePerTwo} money", $"{dividePerTwo}|{valueName}");
        selectMenu.AddOption($"{maxValue} {valueName} XP - {maxValue} money", $"{maxValue}|{valueName}");
    }
    private MessageComponent GetShopComponents()
    {
        ComponentBuilder builder = new ComponentBuilder();
        
        SelectMenuBuilder selectMenuBuilder = new();
        selectMenuBuilder.WithCustomId(Id + "|buy_xp")
            .WithPlaceholder("Select an amount of xp")
            .WithType(ComponentType.SelectMenu);
        
        int combatXp = PlayerAccess.GetNextExpRequiredForNextLevel(m_player.Name, "Combat").Result;
        int explorationXp = PlayerAccess.GetNextExpRequiredForNextLevel(m_player.Name, "Exploration").Result;
        AddShopSelectMenuValues(selectMenuBuilder,combatXp, "Combat");
        AddShopSelectMenuValues(selectMenuBuilder,explorationXp, "Exploration");
        
        ButtonBuilder backButtonBuilder = new();
        backButtonBuilder.WithLabel("Back")
            .WithCustomId(Id + "|backShop")
            .WithStyle(ButtonStyle.Secondary);
        
        builder.WithSelectMenu(selectMenuBuilder)
            .WithButton(backButtonBuilder);
        
        return builder.Build();
    }
    
    private void WriteShopMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("Welcome to the shop! You have " + m_player.Money + " money.");
        embed.WithColor(Color.Green);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("shop");
            
        properties.Embed = embed.Build();
        properties.Components = GetShopComponents();
    }
    
    private bool OnShopBack(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteInitialMessage).Wait();
        return true;
    }

    private bool OnShopBuyXp(SocketMessageComponent component)
    {
        
        string[] values = component.Data.Values.First().Split('|');
        m_player = PlayerAccess.GetOrCreatePlayer(m_player.Name).Result;
        Int32.TryParse(values[0], out int expGained);
        
        ShopMessageBuilder shopMessageBuilder = new(expGained, values[1]);
        
        if (expGained > m_player.Money)
        {
            shopMessageBuilder.BuySuccessful = false;
            shopMessageBuilder.moneyValue = m_player.Money;
        }
        else
        {
            shopMessageBuilder.BuySuccessful = true;
            shopMessageBuilder.moneyValue = m_player.Money - expGained;

            PlayerAccess.UpdatePlayerMoney(m_player.Name, -expGained).Wait();
            switch (values[1]) {
                case "Combat":
                    PlayerAccess.UpdatePlayerCombatStats(m_player.Name, expGained).Wait();
                    break;
                case "Exploration":
                    PlayerAccess.UpdatePlayerExplorationStats(m_player.Name, expGained).Wait();
                    break;
                default:
                    throw new InvalidDataException("Invalid shop type.");
            }
        }
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(shopMessageBuilder.WriteBuyMessage).Wait();
        return true;
    }
    
    private bool OnShop(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteShopMessage).Wait();
        return true;
    }
    
    // MOVE---------------------------------------------------------------------------------------
    private class AreaMoveMessageBuilder(Area area)
    {
        public void WriteMoveMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Game");
            embed.WithDescription("You moved to " + area.Name + " !");
            embed.WithColor(Color.Red);
            embed.ImageUrl = ImageLinkHelper.GetImageLink("move");
            
            properties.Embed = embed.Build();
        }
    }
    Area[] GetAreas()
    {
        List<Area> areas = AreaAccess.GetAllAreas().Result;
        return areas.Where(x => x.RequiredLvl <= PlayerAccess.GetPlayerMeanLevel(m_player.Name).Result && x.Id != m_player.AreaId).ToArray();
    }

    private MessageComponent GetMoveComponents(Area[] areas)
    {
        ComponentBuilder builder = new ComponentBuilder();
        
        SelectMenuBuilder selectMenuBuilder = new();
        selectMenuBuilder.WithCustomId(Id + "|move_to")
            .WithPlaceholder("Select a location to move")
            .WithType(ComponentType.SelectMenu);
        
        foreach (var area in areas)
        {
            selectMenuBuilder.AddOption(area.Name, area.Name);
        }
        
        ButtonBuilder backButtonBuilder = new();
        backButtonBuilder.WithLabel("Back")
            .WithCustomId(Id + "|backMove")
            .WithStyle(ButtonStyle.Secondary);
        
        builder.WithSelectMenu(selectMenuBuilder)
            .WithButton(backButtonBuilder);
        
        return builder.Build();
    }
    
    private void WriteMoveMessage(MessageProperties properties)
    {
        Area[] areas = GetAreas();
        if (areas.Length == 0)
        {
            EmbedBuilder wrongEmbed = new();
            wrongEmbed.WithTitle("Game");
            wrongEmbed.WithDescription("You can't move anywhere!");
            wrongEmbed.WithColor(Color.Red);
            wrongEmbed.ImageUrl = ImageLinkHelper.GetImageLink("move");
            
            properties.Embed = wrongEmbed.Build();
            properties.Components = Components;
            return;
        }
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You are currently in " + AreaAccess.GetAreaById(m_player.AreaId).Result.Name + " !");
        embed.WithColor(Color.Red);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("move");
        
        properties.Embed = embed.Build();
        properties.Components = GetMoveComponents(areas);
    }
    
    private bool OnMove(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteMoveMessage).Wait();
        return true;
    }

    private bool OnMoveChoose(SocketMessageComponent component)
    {
        var areaName = component.Data.Values.First();
        PlayerAccess.UpdatePlayerAreaByName(m_player.Name, areaName).Wait();
        m_player = PlayerAccess.GetOrCreatePlayer(m_player.Name).Result;
        
        AreaMoveMessageBuilder moveMessageBuilder = new(AreaAccess.GetAreaByName(areaName).Result);
        
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(moveMessageBuilder.WriteMoveMessage).Wait();
        return true;
    }
    
    private bool OnMoveBack(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteInitialMessage).Wait();
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