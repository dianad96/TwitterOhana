using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace TwitterOhana.Services
{
    public interface ITweetinviService
    {
        string TwitterAuth();
        Models.User ValidateTwitterAuth();
        string SendTweet(string newTweet);
        List<Models.Tweet> SearchTweet(string searchTweet);
        List<Models.Tweet> GetUserTweets();
        List<Models.User> GetUserFollowers();
        string DeleteTweet(long id);
        List<Models.Trend> GetTrends();
    }
}