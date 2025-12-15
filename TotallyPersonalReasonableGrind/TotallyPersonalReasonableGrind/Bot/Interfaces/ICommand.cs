using System.Threading.Tasks;
using Discord.WebSocket;

namespace TotallyPersonalReasonableGrind.Bot.Interfaces;

public interface ICommand
{
    static ulong Index = 0;
    public ulong Id { get; }
    
    public Task<bool> OnSlashCommand(SocketSlashCommand command);
    public Task<bool> OnComponent(SocketMessageComponent component);
}