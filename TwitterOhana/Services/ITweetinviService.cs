using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Tweetinvi.Models;

namespace TwitterOhana.Services
{
    public interface ITweetinviService
    {
        string TwitterAuth();
        Models.User ValidateTwitterAuth(string verifierCode, string authorizationId);
        string SendTweet(string newTweet);
        List<Models.Tweet> SearchTweet(string searchTweet);
        List<Models.Tweet> GetUserTweets();
        List<Models.User> GetUserFollowers();
        string DeleteTweet(long id);
        List<Models.Trend> GetTrends();
    }
}