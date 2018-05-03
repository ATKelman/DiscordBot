using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using DiscordManager.Commands;

namespace DiscordManager.Handlers
{
    public class ReactionHandler : HandlerBase
    {
        public ReactionHandler(DiscordSocketClient c)
            : base(c)
        {
        }

        public override async Task Install(DiscordSocketClient c)
        {
            _client = c;
            _client.ReactionAdded += HandleReactionAdded;
            _client.ReactionRemoved += HandleReactionRemoved;
        }

        private async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            PollingCommand.DecreaseVote(reaction.Emote.Name);
        }

        private async Task HandleReactionAdded(Discord.Cacheable<Discord.IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var emote = reaction.Emote.Name;
            PollingCommand.IncreaseVote(reaction.Emote.Name);
        }
    }
}
