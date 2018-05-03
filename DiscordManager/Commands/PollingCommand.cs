using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DiscordManager.Classes;
using System.Text;
using System;

namespace DiscordManager.Commands
{
    public class PollingCommand : ModuleBase
    {
        public static Poll poll = null;

        [Command("poll", RunMode = RunMode.Async)]
        public async Task CreatePoll([Remainder]string input)
        {
            if(poll != null)
            {
                var msg = string.Format("There is already a poll in progress, the current poll must be completed before starting another. \n Current poll : {0}", poll.name);
                await ReplyAsync(msg);
                return;
            }

            var markPos = input.IndexOf('?');
            var question = input.Substring(0, markPos + 1);
            var options = input.Substring(markPos + 1, input.Length - (markPos + 1)).Split(',');

            poll = new Poll(question, options);
            if(!poll.IsValidOptionsAmount())
            {
                var msg = string.Format("Invalid options amount {0}, there should be between 1 - 10 options", poll.options.Length);
                await ReplyAsync(msg);
                return;
            }

            StringBuilder sbMsg = new StringBuilder();
            sbMsg.Append("A new poll has been started")
                .Append("\n")
                .AppendFormat("Question: {0}", poll.name)
                .Append("\n\n");

            for (int i = 0; i < poll.options.Length; i++)
            {
                sbMsg.Append("\t")
                    .AppendFormat(":{0}: \t ::  \t {1}", poll.GetNumberAsWord(i), poll.options[i])
                    .Append("\n");
            }

            sbMsg.Append("\n")
                .Append("The poll can be ended by calling !endpoll");

            await ReplyAsync(sbMsg.ToString());
        }

        [Command("endpoll", RunMode = RunMode.Async)]
        public async Task EndPoll()
        {
            if(poll == null)
            {
                await ReplyAsync("There is currently no poll in progress");
                return;
            }

            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("Poll Complete! - {0}", poll.name)
                .Append("\n")
                .Append("Results:")
                .Append("\n");

            for (int i = 0; i < poll.options.Length; i++)
            {
                sbMsg.Append("\t")
                    .AppendFormat("{1} vote(s) \t :: \t {0}", poll.options[i], poll.votes[i])
                    .Append("\n");
            }

            await ReplyAsync(sbMsg.ToString());

            poll = null;
        }

        public static void IncreaseVote(string reaction)
        {
            if(poll == null) { return; }

            poll.AdjustVote(reaction, 1);
        }

        public static void DecreaseVote(string reaction)
        {
            if(poll == null) { return; }

            poll.AdjustVote(reaction, -1);
        }
    }
}
