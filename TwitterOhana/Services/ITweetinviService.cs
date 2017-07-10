using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Tweetinvi.Models;

namespace TwitterOhana.Services
{
    public interface ITweetinviService
    {
        string TwitterAuth();
        IAuthenticatedUser ValideTwitterAuth(HttpRequest request);
        string SendTweet(string newTweet);
        List<Models.Tweet> SearchTweet(string searchTweet);
        List<Models.Tweet> GetUserTweets();
        List<Models.User> GetUserFollowers();
        string DeleteTweet(long id);
    }
}