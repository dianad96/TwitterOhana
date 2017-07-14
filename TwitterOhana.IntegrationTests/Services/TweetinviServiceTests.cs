using System;
using System.Net;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using TwitterOhana.Controllers;
using TwitterOhana.Services;
using Xunit;

namespace TwitterOhana.IntegrationTests.Services
{
    public class Credentials : ITwitterCredentials
    {
        public Credentials()
        {
            AccessToken = "696780677870706688-6W9L7oMQ9HUF2n0lQAOFfmbMuuNQKOg";
            AccessTokenSecret = "4DHg9ZJ2eu6dRFiEmOijLrwBeLjliqfh60ej0vOlUuOhI";
            ConsumerKey = "K0CvfPNmTrCc0djczVyxHz0xz";
            ConsumerSecret = "w3EG7V7g7GrtEm1LND1vNiKooc2zwTJRkM1XhPy2AmioRpX6kk";
        }

        IConsumerCredentials IConsumerCredentials.Clone()
        {
            return Clone();
        }

        public bool AreSetupForUserAuthentication()
        {
            return true;
        }

        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public ITwitterCredentials Clone()
        {
            throw new System.NotImplementedException();
        }

        public bool AreSetupForApplicationAuthentication()
        {
            return true;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string ApplicationOnlyBearerToken { get; set; }

    }

    public class TweetinviServiceTests
    {
        [Fact]
        public void when_calling_valid_twitter_auth_we_should_return_a_user()
        {
            var tweet = $"tweet_{DateTime.Now.Ticks}";
            var subject = new TweetinviService(new Credentials());

            var returnedTweet = subject.SendTweet(tweet);

            Assert.Equal(tweet, returnedTweet);
        }
    }
}
