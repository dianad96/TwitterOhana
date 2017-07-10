using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterOhana.Models
{
    public class User
    {
        public String Name { get; set; }
        public int StatusesCount { get; set; }
        public int FollowersCount { get; set; }
        public String ProfileImage { get; set; }
        public int FriendsCount { get; set; }
        public String ScreenName { get; set; }
        public String BackgroundImage { get; set; }
    }
}
