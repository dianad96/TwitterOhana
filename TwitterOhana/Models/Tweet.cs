using System;
using Tweetinvi.Models;

namespace TwitterOhana.Models
{
    public class Tweet
    {
        public String Text { get; set; }
        public String UserName { get; set; }
        public String ScreenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public String UserProfileImage { get; set; }
        public long Id { get; set; }
        public ITweet UserTweet { get; set; }
    }
}
