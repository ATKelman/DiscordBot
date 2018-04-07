using RedditSharp;
using RedditSharp.Things;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditManager
{
    public class RedditHandler
    {
        public RedditHandler()
        {
            Login();
        }

        private void Login()
        {
            bool authenticated = false;
            Reddit reddit = null;
            reddit = new Reddit(ConfigurationManager.AppSettings["RedditToken"]);
            reddit.InitOrUpdateUserAsync();
            authenticated = reddit.User != null;
            //Subreddit sub = null;
            if (!authenticated)
            {
                var sub = reddit.GetSubredditAsync("anime");

                //var threads = 
            }
        }
    }
}
