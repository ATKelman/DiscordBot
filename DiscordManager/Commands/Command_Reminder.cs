using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Runtime.InteropServices;
using DiscordManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordManager.Commands
{
    public class Command_Reminder : ModuleBase
    {
        [Command("info", RunMode = RunMode.Async)]
        public async Task Info()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription}{RuntimeInformation.OSArchitecture}\n\n" +
                $"- This bot is a work in progress"
                );
        }

        [Command("remind", RunMode = RunMode.Async)]
        public async Task Remind(params string[] str)
        {
            try
            {
                var time = Convert.ToInt32(str[0]);
                var datetime = DateTime.Now.AddMinutes(time);

                string message = "";
                for (int i = 1; i < str.Length; i++)
                {
                    message += str[i] + " ";
                }

                SetReminder(Context.User.Mention, Context.Channel.Id.ToString(), datetime, message);

                var msg = string.Format("{0} Reminder Set!", Context.User.Mention);
                await ReplyAsync(msg);
            }
            catch
            {
                await ReplyAsync(string.Format("Cannot convert {0} to minutes", str[0]));
            }
        }

        private void SetReminder(string user, string channel, DateTime time, string message)
        {
            using (var database = new DiscordBotEntities())
            {
                var reminder = new Reminder
                {
                    Username = user,
                    Message = message,
                    ReminderDate = time,
                    Channel = channel
                };

                database.Reminders.Add(reminder);
                database.SaveChanges();
            }
        }

        public static List<Reminder> GetElapsedReminders()
        {
            var dtNow = DateTime.Now;
            using (var db = new DiscordBotEntities())
            {
                var elapsedReminders = db.Reminders.Where(x => x.ReminderDate < dtNow).ToList();
                return elapsedReminders;
            }      
        }
    }
}
