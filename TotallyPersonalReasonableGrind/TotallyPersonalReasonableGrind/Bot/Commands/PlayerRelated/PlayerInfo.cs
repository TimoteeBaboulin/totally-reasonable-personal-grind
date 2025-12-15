using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

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
        //Get Player Info
        Task<Player> player = PlayerAccess.GetOrCreatePlayer(command.User.GlobalName);
        player.Wait();
        Player p = player.Result;
        
        // Display Player Info
        EmbedBuilder embed = new();
        embed.WithTitle("Player Info");
        embed.WithAuthor(command.User.GlobalName, command.User.GetAvatarUrl());
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat Xp").WithValue(p.CombatEXP).WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat LvL").WithValue(p.CombatLVL).WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("ㅤ").WithValue("ㅤ").WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration Xp").WithValue(p.ExplorationEXP).WithIsInline(true));
        embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration LvL").WithValue(p.ExplorationLVL).WithIsInline(true));
        command.RespondAsync(embed: embed.Build());
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        return Task.FromResult(false);
    }
}