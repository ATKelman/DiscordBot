using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using DiscordManager.Database;
using System.Linq;

namespace DiscordManager.Handlers
{
    public class CommandHandler : HandlerBase
    {
        private static CommandService _cmds;

        public CommandHandler(DiscordSocketClient c)
            : base(c)
        {
        }

        public override async Task Install(DiscordSocketClient c)
        {
            _client = c;
            _cmds = new CommandService();

            _client.MessageReceived += HandleCommand;
            //_client.MessageReceived += HandleMention;

            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly());


        }

        public async Task HandleCommand(SocketMessage e)
        {
            SocketUserMessage msg = e as SocketUserMessage;
            if (msg == null) return;
            CommandContext context = new CommandContext(_client, msg);
            if (IsBot(context)) return; 

            int argPos = 0;
            if (msg.HasStringPrefix("!", ref argPos))
            {
                var result = await _cmds.ExecuteAsync(context, argPos);

                if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ErrorReason.ToString());
            }
        }



        public static CommandService GetCommandService()
        {
            return _cmds;
        }

        //private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
        //{
        //    return text.Where(x => x >= min && x <= max);
        //}

        //var hiragana = GetCharsInRange(context.Message.ToString(), 0x3040, 0x309F);
        //if (hiragana.Count() > 0)
        //{
        //    Console.WriteLine("found hiragana");
        //    Console.WriteLine(msg.Content);
        //    //Console.WriteLine(msg2.Content);
        //    Console.WriteLine(context.ToString());

        //    var result = await _cmds.ExecuteAsync(context, 0);

        //    if (!result.IsSuccess) await context.Channel.SendMessageAsync(result.ErrorReason.ToString());
        //}



        /*
        var romaji = GetCharsInRange(searchKeyword, 0x0020, 0x007E);
        var hiragana = GetCharsInRange(searchKeyword, 0x3040, 0x309F);
        var katakana = GetCharsInRange(searchKeyword, 0x30A0, 0x30FF);
        var kanji = GetCharsInRange(searchKeyword, 0x4E00, 0x9FBF);
         */
    }
}
