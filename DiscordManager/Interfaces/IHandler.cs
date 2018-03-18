using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordManager.Interfaces
{
    interface IHandler
    {
        Task Install(DiscordSocketClient c);
    }
}
