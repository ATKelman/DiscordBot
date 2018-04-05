using System.Threading.Tasks;
using Discord.WebSocket;
using System;

namespace DiscordManager.Handlers
{
    public class ReminderHandler : HandlerBase
    {
        private System.Timers.Timer timer;

        public ReminderHandler(DiscordSocketClient c)
            : base(c)
        {
        }

        public override async Task Install(DiscordSocketClient c)
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
            timer.Start();
            await Commands.Command_Reminder.HandleRemindersAsync(_client);
        }

        private async Task HandleReminder(DiscordManager.Database.Reminder reminder)
        {
            var channel = _client.GetChannel(Convert.ToUInt64(reminder.Channel)) as ISocketMessageChannel;
            var msg = string.Format("{0} {1}", reminder.Username, reminder.Message);
            await channel.SendMessageAsync(msg);
        }

    }
}
