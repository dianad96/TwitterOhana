using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using TwitterOhana.Controllers;
using Tweet = Tweetinvi.Tweet;
using User = Tweetinvi.User;

namespace TwitterOhana.Services
{
    public class TweetinviService : ITweetinviService
    {
        private ITwitterCredentials _userCredentials = null;

        public TweetinviService()
        {
            
        }

        public TweetinviService(ITwitterCredentials userCredentials = null)
        {
            _userCredentials = userCredentials;
        }

        public string TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(MyCredentials.ConsumerKey, MyCredentials.ConsumerSecret);
            IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(appCreds, MyCredentials.RedirectUrl);
            return authenticationContext.AuthorizationURL;
        }

        public Models.User ValidateTwitterAuth(string verifierCode, string authorizationId)
        {
            _userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);
            var user = User.GetAuthenticatedUser(_userCredentials);

            var model = new Models.User();
            model.Name = user.Name;
            model.ScreenName = user.ScreenName;
            model.FollowersCount = user.FollowersCount;
            model.FriendsCount = user.FriendsCount;
            model.StatusesCount = user.StatusesCount;
            model.ProfileImage = user.ProfileImageUrl;
            model.BackgroundImage = user.ProfileBannerURL;

            return model;
        }

        public string SendTweet(string newTweet)
        {
            var user = User.GetAuthenticatedUser(_userCredentials);
            var tweet = user.PublishTweet(newTweet);
            return !tweet.Text.IsNullOrEmpty() ? newTweet : "An error has occured!";
        }

        public List<Models.Trend> GetTrends()
        {
            var modelTrend = new List<Models.Trend>();

            var result = Auth.ExecuteOperationWithCredentials(_userCredentials, () =>
            {
                var trends = Trends.GetTrendsAt(44418);

                var newTrends = trends.Trends;
                IEnumerator<ITrend> e = newTrends.GetEnumerator();
                int ok = 0;
                while (e.MoveNext()&&ok<10)
                {
                    List<Models.Tweet> tweets = SearchTweet(e.Current.Name);

                    modelTrend.Add(new Models.Trend()
                    {
                        Name = e.Current.Name,
                        URL = e.Current.URL,
                        Query = e.Current.Query,
                        PromotedContent = e.Current.PromotedContent,
                        TweetVolume = e.Current.TweetVolume,
                        Tweet = tweets
                     });
                    ok++;
                }
                return modelTrend;
            });
            return result;
        }

        public List<Models.Tweet> SearchTweet(string searchTweet)
        {
            var matchingTweets = Search.SearchTweets(searchTweet);
            var model = new List<Models.Tweet>();

            IEnumerator<ITweet> e = matchingTweets.GetEnumerator();
            while (e.MoveNext())
            {
                model.Add(new Models.Tweet
                {
                    Text = e.Current.Text,
                    UserName = e.Current.CreatedBy.Name,
                    ScreenName = e.Current.CreatedBy.ScreenName,
                    CreatedAt = e.Current.CreatedAt,
                    UserProfileImage = e.Current.CreatedBy.ProfileImageUrl
                });

                string value = e.Current.Text;
                Console.WriteLine(value);
            }
            return model;
        }

        public List<Models.Tweet> GetUserTweets()
        {
            var user = User.GetAuthenticatedUser(_userCredentials);
            var userTweets = user.GetHomeTimeline();
            var model = new List<Models.Tweet>();

            IEnumerator<ITweet> e = userTweets.GetEnumerator();
            while (e.MoveNext())
            {
                model.Add(new Models.Tweet
                {
                    Text = e.Current.Text,
                    CreatedAt = e.Current.CreatedAt,
                    UserProfileImage = e.Current.CreatedBy.ProfileImageUrl,
                    UserName = e.Current.CreatedBy.Name,
                    ScreenName = e.Current.CreatedBy.ScreenName,
                    Id = e.Current.Id,
                    UserTweet = e.Current
                });
            }
            return model;
        }

        public List<Models.User> GetUserFollowers()
        {
            var model = new List<Models.User>();

            var result = Auth.ExecuteOperationWithCredentials(_userCredentials, () =>
            {
                // Get the first 250 followers of the user
                var followers = User.GetFollowers("");
                IEnumerator<IUser> e = followers.GetEnumerator();
                while (e.MoveNext())
                {
                    model.Add(new Models.User
                    {
                        Name = e.Current.Name,
                        ScreenName = e.Current.ScreenName,
                        StatusesCount = e.Current.StatusesCount,
                        FollowersCount = e.Current.FollowersCount,
                        FriendsCount = e.Current.FriendsCount,
                        ProfileImage = e.Current.ProfileImageUrl,
                        BackgroundImage = e.Current.ProfileBannerURL
                    });
                }
                return model;
            });

            return result;
        }

        public string DeleteTweet(long id)
        {
            var success = Auth.ExecuteOperationWithCredentials(_userCredentials, () =>
            {
                ITweet toDelete = Tweet.GetTweet(id);
                var tweet = toDelete.Destroy();
                return tweet;
            });

            if (success)
                return "deleted";
            else
                return "failed to delete";
        }
    }
}
