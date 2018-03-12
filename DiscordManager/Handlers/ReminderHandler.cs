using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System.Timers;
using System;
using System.Linq;
using DiscordManager.Database;
using System.Threading;

namespace DiscordManager.Handlers
{
    class ReminderHandler
    {
        private DiscordSocketClient _client;
        private System.Timers.Timer timer;

        public void Install(DiscordSocketClient c)
        {
            _client = c;
            StartTimer();
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer
            {
                Interval = 20000
            };
            timer.Elapsed += CheckReminders;

            timer.Enabled = true;
        }

        private async void CheckReminders(Object source, System.Timers.ElapsedEventArgs e)
        {
            await DiscordManager.Commands.Command_Reminder.HandleRemindersAsync(_client);
        }

        private async Task HandleReminder(DiscordManager.Database.Reminder reminder)
        {
            var channel = _client.GetChannel(Convert.ToUInt64(reminder.Channel)) as ISocketMessageChannel;
            var msg = string.Format("{0} {1}", reminder.Username, reminder.Message);
            await channel.SendMessageAsync(msg);
        }

    }
}
