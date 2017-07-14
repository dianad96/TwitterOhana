using System;
using Tweetinvi.Models;

namespace TwitterOhana.Models
{
    public class Tweet
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserProfileImage { get; set; }
        public long Id { get; set; }
        public ITweet UserTweet { get; set; }
    }
}
