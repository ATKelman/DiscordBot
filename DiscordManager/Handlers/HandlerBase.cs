using Discord.WebSocket;
using DiscordManager.Interfaces;
using System.Threading.Tasks;

namespace DiscordManager.Handlers
{
    public abstract class HandlerBase : IHandler
    {
        public DiscordSocketClient _client;

        public HandlerBase(DiscordSocketClient c)
        {
            Install(c);
        }

        abstract public Task Install(DiscordSocketClient c);
    }
}
