using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using TwitterOhana.Controllers;

namespace TwitterOhana.Services
{
    public class TweetinviService : ITweetinviService
    {
        public string TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(MyCredentials.ConsumerKey, MyCredentials.ConsumerSecret);
            IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(appCreds, MyCredentials.RedirectUrl);
            string url = authenticationContext.AuthorizationURL;
            return url;
        }

        public IAuthenticatedUser ValideTwitterAuth(HttpRequest request)
        {
            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(request.Query.ElementAt(2).Value, request.Query.ElementAt(0).Value);
            MyCredentials.MyCreds = userCreds;
            var user = User.GetAuthenticatedUser(userCreds);
            MyCredentials.MyScreenName = user.ScreenName;
            return user;
        }
        public string SendTweet(String newTweet)
        {
            var user = User.GetAuthenticatedUser(MyCredentials.MyCreds);
            var tweet = user.PublishTweet(newTweet);
            if (!tweet.Text.IsNullOrEmpty())
                return "Sent!";
            else
                return "An error has occured!";
        }

        public List<Models.Tweet> SearchTweet(String searchTweet)
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

                String value = e.Current.Text;
                Console.WriteLine(value);
            }
            return model;
        }

        public List<Models.Tweet> GetUserTweets()
        {
            var user = User.GetAuthenticatedUser(MyCredentials.MyCreds);
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

            var result = Auth.ExecuteOperationWithCredentials(MyCredentials.MyCreds, () =>
            {
                // Get the first 250 followers of the user
                var followers = User.GetFollowers(MyCredentials.MyScreenName);
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
            var success = Auth.ExecuteOperationWithCredentials(MyCredentials.MyCreds, () =>
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
