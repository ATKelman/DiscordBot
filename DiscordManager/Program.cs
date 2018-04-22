using System;
using System.Configuration;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordManager.Handlers;

namespace DiscordManager
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        private CommandHandler _commands;
        private ReminderHandler _reminder;
        private MentionHandler _mention;

        public async Task Start()
        {
            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["Token"]);
            await _client.StartAsync();

            _client.Log += Log;

            InitializeHandlers(_client);
            await Task.Delay(-1);
        }

        private void InitializeHandlers(DiscordSocketClient c)
        {
            _commands = new CommandHandler(c);
            _reminder = new ReminderHandler(c);
            _mention = new MentionHandler(c);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return null;
        }
    }
}
