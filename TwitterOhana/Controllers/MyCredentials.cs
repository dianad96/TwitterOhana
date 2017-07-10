using System;
using Tweetinvi.Models;

namespace TwitterOhana.Controllers
{
    public static class MyCredentials
    {
        public static string ConsumerKey = "K0CvfPNmTrCc0djczVyxHz0xz";
        public static string ConsumerSecret = "w3EG7V7g7GrtEm1LND1vNiKooc2zwTJRkM1XhPy2AmioRpX6kk";
        public static String RedirectUrl = "http://localhost:2559/Home/ValidateTwitterAuth";
        public static String MyScreenName;
        public static ITwitterCredentials MyCreds;
    }
}
