using Discord.WebSocket;

namespace TotallyPersonalReasonableGrind.Bot.Interfaces;

public interface ICommand
{
    public static abstract SocketApplicationCommand BuildProperties();
    
    public ulong Id { get; }
    
    public abstract Task<bool> OnSlashCommand(SocketSlashCommand command);
    public abstract Task<bool> OnComponent(SocketMessageComponent component);
}