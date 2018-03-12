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

        public async Task Start()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandHandler();
            _reminder = new ReminderHandler();

            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["Token"]);
            //await _client.LoginAsync(TokenType.Bot, "MzE5ODk1ODM2ODM2MzY0Mjk5.DBHluw.CckE04fI_3xLwlu0SPzXDKlPHAQ");
            await _client.StartAsync();

            _client.Log += Log;

            _reminder.Install(_client);
            await _commands.Install(_client);
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return null;
        }
    }
}
