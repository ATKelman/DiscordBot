using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using DiscordManager.Database;
using System.Linq;

namespace DiscordManager.Handlers
{
    public class MentionHandler : HandlerBase
    {
        public MentionHandler(DiscordSocketClient c)
            : base(c)
        {
        }

        public override async Task Install(DiscordSocketClient c)
        {
            _client = c;
            _client.MessageReceived += HandleMention;
        }

        public async Task HandleMention(SocketMessage e)
        {
            SocketUserMessage msg = e as SocketUserMessage;
            if (msg == null) return;
            CommandContext context = new CommandContext(_client, msg);
            if (IsBot(context))  return; 

            var users = msg.MentionedUsers;
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    using (var db = new DiscordBotEntities())
                    {
                        var status = db.UserStatus.Where(x => x.User == user.Mention).Select(x => x.Status).SingleOrDefault();
                        if (status != null)
                        {
                            if (!status.ToLower().Equals("none"))
                            {
                                var message = string.Format("{0} is currently {1}", user.Username, status);
                                await context.Channel.SendMessageAsync(message);
                            }
                        }
                    }
                }
            }
        }
    }
}
