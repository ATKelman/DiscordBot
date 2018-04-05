using Discord;
using Discord.Commands;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DiscordManager.Commands
{
    public class GeneralCommand : ModuleBase
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
    }
}
