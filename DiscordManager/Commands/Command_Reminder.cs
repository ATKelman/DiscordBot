using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Runtime.InteropServices;
using DiscordManager.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using System.Data.Entity.Validation;

namespace DiscordManager.Commands
{
    public class Command_Reminder : ModuleBase
    {
        [Command("remind", RunMode = RunMode.Async)]
        [Alias("RemindMe", "remindme", "reminder")]
        public async Task Remind(params string[] str)
        {
            try
            {
                var startIndex = 1;
                DateTime datetime = DateTime.Now;
                if(str[0].Contains(':'))
                {
                    try
                    {
                        datetime = HandleReminderDate(str[0]);
                    }
                    catch
                    {
                        throw new Exception(string.Format("Could not convert {0} to DateTime, please use the proper format of: \n\t ![Command] [DateTime] [Message]", str[0]));
                    }
                }
                else if(str[0].Contains('h') || str[0].Contains('H'))
                {
                    try
                    {
                        datetime = DateTimeAddHours(str, out int skips);
                        startIndex = skips;
                    }
                    catch
                    {
                        throw new Exception(string.Format("Could not convert {0} to int, please use the proper format of: \n\t ![Command] [int]H [Message] \n OR \n\t ![Command] [int]H [int]M [Message]", str[0]));
                    }
                }
                else if(str[0].Contains('m') || str[0].Contains('M'))
                {
                    try
                    {
                        datetime = DatetimeAddMinues(str[0], datetime);
                    }
                    catch
                    {
                        throw new Exception(string.Format("Could not convert {0} to int, please use the proper format of: \n\t ![Command] [int]M [Message]", str[0]));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Could not find a suitable time format!"));
                }

                string message = "";
                for (int i = startIndex; i < str.Length; i++)
                {
                    message += str[i] + " ";
                }

                SetReminder(Context.User.Mention, Context.Channel.Id.ToString(), datetime, message);

                var msg = string.Format("{0} Reminder Set!", Context.User.Mention);
                await ReplyAsync(msg);
            }
            catch(Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }

        private DateTime HandleReminderDate(string time)
        {
            return DateTime.Parse(time);
        }

        private DateTime DateTimeAddHours(string[] str, out int skips)
        {
            skips = 1;
            var hourInstance = str[0].Remove(str[0].Length - 1, 1);
            var hours = Convert.ToInt32(hourInstance);
            var datetime = DateTime.Now.AddHours(hours);

            if (str[1].Contains('m') || str[1].Contains('M'))
            {
                try
                {
                    datetime = DatetimeAddMinues(str[1], datetime);
                    skips = 2;
                }
                catch
                {
                    // do nothing
                }
            }

            return datetime;
        }

        private DateTime DatetimeAddMinues(string time, DateTime datetime)
        {
            var minInstance = time.Remove(time.Length - 1, 1);
            var minutes = Convert.ToInt32(minInstance);
            return datetime.AddMinutes(minutes);
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
                    Channel = channel,
                    Status = 10
                };

                database.Reminders.Add(reminder);

                try
                {
                    database.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
        }

        public static async Task HandleRemindersAsync(DiscordSocketClient _client)
        {
            using (var db = new DiscordBotEntities())
            {
                var dtNow = DateTime.Now;
                var elapsedReminders = db.Reminders.Where(x => x.ReminderDate < dtNow && (x.Status == 10 || x.Status == 20)).ToList();

                if (elapsedReminders.Any())
                {
                    foreach (var reminder in elapsedReminders)
                    {
                        if(reminder.Status == 10)
                        {           
                            reminder.Status = 100;
                        }
                        else if(reminder.Status == 20)
                        {
                            reminder.ReminderDate = reminder.ReminderDate.AddDays(1);
                        }

                        await SendReminder(reminder, _client);
                    }
                }
                db.SaveChanges();
            }
        }

        private static async Task SendReminder(Reminder reminder, DiscordSocketClient _client)
        {
            try
            {
                var channel = _client.GetChannel(Convert.ToUInt64(reminder.Channel)) as ISocketMessageChannel;
                var msg = string.Format("{0} {1}", reminder.Username, reminder.Message);
                await channel.SendMessageAsync(msg);
            }
            catch
            {
                Console.WriteLine("Unable to retrieve channel");
            }
        }
    }
}
