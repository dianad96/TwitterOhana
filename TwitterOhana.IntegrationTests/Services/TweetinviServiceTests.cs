using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using TwitterOhana.Controllers;
using TwitterOhana.Services;
using Xunit;

namespace TwitterOhana.IntegrationTests.Services
{
    public class Credentials : ICredentialService
    {

        public Credentials()
        {
            AccessToken = "696780677870706688-6W9L7oMQ9HUF2n0lQAOFfmbMuuNQKOg";
            AccessTokenSecret = "4DHg9ZJ2eu6dRFiEmOijLrwBeLjliqfh60ej0vOlUuOhI";
            ConsumerKey = "K0CvfPNmTrCc0djczVyxHz0xz";
            ConsumerSecret = "w3EG7V7g7GrtEm1LND1vNiKooc2zwTJRkM1XhPy2AmioRpX6kk";
        }

        public string ConsumerKey { get; private set; }
        public string AccessTokenSecret { get; private set; }
        public string AccessToken { get; private set; }
        public string ConsumerSecret { get; private set; }

        public string getConsumerKey()
        {
            return ConsumerKey;
        }

        public string getConsumerSecret()
        {
            return ConsumerSecret;
        }

        public string getRedirectUrl()
        {
            throw new NotImplementedException();
        }

        public ITwitterCredentials GetUserCredentials()
        {
            return new TwitterCredentials(ConsumerKey,ConsumerSecret,AccessToken,AccessTokenSecret);
        }
    }

    public class TweetinviServiceTests
    {
        [Fact]
        public void when_calling_send_tweet_we_should_return_a_tweet()
        {
            var tweet = $"tweet_{DateTime.Now.Ticks}";
            var subject = new TweetinviService(new Credentials());
            var returnedTweet = subject.SendTweet(tweet);
            Assert.Equal(tweet, returnedTweet);
        }

        [Fact]
        public void when_calling_delete_tweet_we_should_delete_the_tweet()
        {
            long id = 2131231;
            var subject = new TweetinviService(new Credentials());
            var returnedResponse = subject.DeleteTweet(id);
            Assert.Equal(returnedResponse, "failed to delete");
        }

        [Fact]
        public void when_calling_get_tweets_we_should_return_user_tweets()
        {
            var subject = new TweetinviService(new Credentials());
            var returnedResponse = subject.GetUserTweets();
            Assert.NotEqual(returnedResponse.Count, 0);
        }

        [Fact]
        public void when_calling_get_followers_we_should_return_user_follwers()
        {
            var subject = new TweetinviService(new Credentials());
            var returnedResponse = subject.GetUserFollowers();
            Assert.NotEqual(returnedResponse.Count, 0);
        }
        /*
        [Fact]
        public void when_calling_search_tweet_we_should_return_list_of_tweets()
        {
            var tweet = "ohana";
            var subject = new TweetinviService(new Credentials());
            var returnedResponse = subject.SearchTweet(tweet);
            Assert.NotEqual(returnedResponse.Count, 0);
        }*/
    }
}
