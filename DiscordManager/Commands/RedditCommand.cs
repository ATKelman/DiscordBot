﻿using Discord;
using Discord.Commands;
using RedditSharp;
using RedditSharp.Things;
using System.Configuration;

namespace DiscordManager.Commands
{
    public class RedditCommand : ModuleBase
    {

        private void Login()
        {
            bool authenticated = false;
            Reddit reddit = null;
            reddit = new Reddit(ConfigurationManager.AppSettings["RedditToken"]);
            reddit.InitOrUpdateUserAsync();
            authenticated = reddit.User != null;
            //Subreddit sub = null;
            if(!authenticated)
            {
                var sub = reddit.GetSubredditAsync("");
            }
        }
    }
}
