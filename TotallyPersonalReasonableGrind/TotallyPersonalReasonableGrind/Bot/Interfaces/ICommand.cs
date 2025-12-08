using Discord.WebSocket;

namespace TotallyPersonalReasonableGrind.Bot.Interfaces;

public interface ICommand
{
    public ulong Id { get; }
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command);
    public Task<bool> OnComponent(SocketMessageComponent component);
}