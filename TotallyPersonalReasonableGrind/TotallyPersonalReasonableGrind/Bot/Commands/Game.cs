using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Helpers;
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

    public ulong Id { get; } = ++ICommand.Index;

    // Action Map--------------------------------------------------------------------------------
    private static Dictionary<string, Func<SocketMessageComponent, bool>>? m_actionMap = null;
    
    private void PopulateActionMap()
    {
        m_actionMap = new Dictionary<string, Func<SocketMessageComponent, bool>>
        {
            { "move", OnMove },
            { "walk", OnWalk },
            { "sell", OnSell },
            { "hit",  OnHit  },
            { "shop", OnShop },
            { "quit", OnQuit }
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
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command)
    {
        command.DeferAsync();
        
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You're actually in the game!");
        command.ModifyOriginalResponseAsync(WriteInitialMessage);
        
        return Task.FromResult(true);
    }

    // WALK---------------------------------------------------------------------------------------
    
    private void WriteWalkMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You walked forward!");
        embed.WithColor(Color.Purple);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("walk");
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    private bool OnWalk(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteWalkMessage).Wait();
        return true;
    }
    
    // SELL---------------------------------------------------------------------------------------
    
    private void WriteSellMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You sold an item!");
        embed.WithColor(Color.Gold);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("sell");
        
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    private bool OnSell(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteSellMessage).Wait();
        return true;
    }
    
    // HIT----------------------------------------------------------------------------------------
    
    private void WriteHitMessage(MessageProperties properties)
    {
        EmbedBuilder embed = new();
        embed.WithTitle("Game");
        embed.WithDescription("You hit the enemy!");
        embed.WithColor(Color.Orange);
        embed.ImageUrl = ImageLinkHelper.GetImageLink("hit");
        
        properties.Embed = embed.Build();
        properties.Components = Components;
    }
    
    private bool OnHit(SocketMessageComponent component)
    {
        RestInteractionMessage msg = component.GetOriginalResponseAsync().Result;
        msg.ModifyAsync(WriteHitMessage).Wait();
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