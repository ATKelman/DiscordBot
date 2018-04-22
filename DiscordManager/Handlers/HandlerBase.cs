using Discord.Commands;
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

        protected bool IsBot(CommandContext context)
        {
            if(context.User.IsBot)
            {
                return true;
            }
            return false;
        }

        abstract public Task Install(DiscordSocketClient c);
    }
}
