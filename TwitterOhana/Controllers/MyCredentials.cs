using System;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Tweetinvi.Models;

namespace TwitterOhana.Controllers
{
    public static class MyCredentials
    {
        public static string CONSUMER_KEY = "K0CvfPNmTrCc0djczVyxHz0xz";
        public static string CONSUMER_SECRET = "w3EG7V7g7GrtEm1LND1vNiKooc2zwTJRkM1XhPy2AmioRpX6kk";
        public static string ACCESS_TOKEN = "ACCESS_TOKEN";
        public static string ACCESS_TOKEN_SECRET = "ACCESS_TOKEN_SECRET";
        public static String redirectURL = "http://localhost:2559/Home/ValidateTwitterAuth";

        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
        }
    }
}
