using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.Core;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Controllers.User;
using Tweetinvi.Credentials;
using Tweetinvi.Logic;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using TwitterOhana.Controllers;
using TwitterOhana.Services;
using Xunit;
using User = TwitterOhana.Models.User;

namespace TwitterOhana.Tests.Controllers
{
    public class home_controller_tests
    {
        [Fact]
        public void when_calling_send_tweet_it_should_return_the_sent_tweet()
        {
            var expectedResult = "Something!";
            var newTweet = "Something else!";

            var tweetinviService = Substitute.For<ITweetinviService>();
            tweetinviService.SendTweet(newTweet).Returns(expectedResult);

            var subject = new HomeController(tweetinviService);          
            var outcome = subject.SendTweet(newTweet);
    
            Assert.Equal(outcome, expectedResult);
        }

        [Fact]
        public void when_calling_delete_tweet_we_should_delete_tweet()
        {
            var newTweetId = 12321;
            var expectedResultDelete = "failed to delete";

            var tweetinviService = Substitute.For<ITweetinviService>();
            tweetinviService.DeleteTweet(newTweetId).Returns(expectedResultDelete);

            var subject = new HomeController(tweetinviService);
            var outcome = subject.DeleteTweet(newTweetId);

            Assert.Equal(outcome, expectedResultDelete);
        }

        [Fact]
        public void when_calling_get_followers_we_should_return_the_followers()
        {
            var expectedResult = new List<User>()
            {
                new User{ ScreenName = "John Smith"}
            };

            var tweetinviService = Substitute.For<ITweetinviService>();
            tweetinviService.GetUserFollowers().Returns(expectedResult);

            var subject = new HomeController(tweetinviService);
            ViewResult outcome = subject.GetUserFollowers();

            Assert.Equal(outcome.Model, expectedResult);

        }

        [Fact]
        public void when_calling_get_tweets_we_should_return_the_tweets()
        {
            var expectedResult = new List<Models.Tweet>()
            {
                new Models.Tweet{ Text = "ohana"}
            };

            var tweetinviService = Substitute.For<ITweetinviService>();
            tweetinviService.GetUserTweets().Returns(expectedResult);

            var subject = new HomeController(tweetinviService);
            ViewResult outcome = subject.GetUserTweets();

            Assert.Equal(outcome.Model, expectedResult);
        }

        [Fact]
        public void when_calling_search_tweets_we_should_return_the_tweets()
        {
            var newTweet = "ohana";
            var expectedResult = new List<Models.Tweet>()
            {
                new Models.Tweet{ Text = "ohana means family"}
            };

            var tweetinviService = Substitute.For<ITweetinviService>();
            tweetinviService.SearchTweet(newTweet).Returns(expectedResult);

            var subject = new HomeController(tweetinviService);
            ViewResult outcome = subject.SearchTweet(newTweet);

            Assert.Equal(outcome.Model, expectedResult);
        }

       
    }
}
