using Discord;
using Discord.Commands;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using DiscordManager.Database;

namespace DiscordManager.Commands
{
    public class StatusCommand : ModuleBase
    {
        [Command("status", RunMode = RunMode.Async)]
        public async Task Status([Remainder]string status)
        {
            if (UserAlreadyExists(Context.User.Mention))
            {
                UpdateStatus(Context.User.Mention, status);
            }
            else
            {
                CreateUser(Context.User.Mention, status);
            }

            await ReplyAsync("Status updated!");
        }

        private bool UserAlreadyExists(string name)
        {
            using (var db = new DiscordBotEntities())
            {
                var userCheck = db.UserStatus.Where(x => x.User == name).SingleOrDefault();
                if(userCheck != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void UpdateStatus(string name, string status)
        {
            using (var db = new DiscordBotEntities())
            {
                var userStatus = db.UserStatus.Where(x => x.User == name).SingleOrDefault();
                userStatus.Status = status;
                db.SaveChanges();
            }
        }

        private void CreateUser(string name, string status)
        {
            using (var db = new DiscordBotEntities())
            {
                var userStatus = new UserStatu();
                userStatus.User = name;
                userStatus.Status = status;

                db.UserStatus.Add(userStatus);
                db.SaveChanges();
            }
        }
    }
}
