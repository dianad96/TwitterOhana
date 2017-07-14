using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterOhana.Models
{
    public class User
    {
        public string Name { get; set; }
        public int StatusesCount { get; set; }
        public int FollowersCount { get; set; }
        public string ProfileImage { get; set; }
        public int FriendsCount { get; set; }
        public string ScreenName { get; set; }
        public string BackgroundImage { get; set; }
    }
}
