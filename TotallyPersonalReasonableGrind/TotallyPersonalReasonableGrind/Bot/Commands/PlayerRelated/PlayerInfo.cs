using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.Commands.PlayerRelated;

public class PlayerInfo : ICommand
{
    private class PlayerInfoMessageBuilder(Player player)
    {
        public void WritePlayerInfoMessage(MessageProperties properties)
        {
            EmbedBuilder embed = new();
            embed.WithTitle("Player Info");
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat Xp").WithValue(player.CombatEXP).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Combat LvL").WithValue(player.CombatLVL).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("ㅤ").WithValue("ㅤ").WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration Xp").WithValue(player.ExplorationEXP).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Exploration LvL").WithValue(player.ExplorationLVL).WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("ㅤ").WithValue("ㅤ").WithIsInline(true));
            embed.Fields.Add(new EmbedFieldBuilder().WithName("Money").WithValue(player.Money).WithIsInline(true));
        }
    }
    
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
        PlayerInfoMessageBuilder builder = new(p);
        command.ModifyOriginalResponseAsync(builder.WritePlayerInfoMessage);
        
        return Task.FromResult(false);
    }

    public Task<bool> OnComponent(SocketMessageComponent component)
    {
        return Task.FromResult(false);
    }
}