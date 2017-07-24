using System;
using System.Collections.Generic;
using Ganss.XSS;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweet = Tweetinvi.Tweet;
using User = Tweetinvi.User;

namespace TwitterOhana.Services
{
    public class TweetinviService : ITweetinviService
    {
        private readonly ICredentialService _credentialService;

        public TweetinviService(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        public string TwitterAuth()
        {
           var appCreds = new ConsumerCredentials(_credentialService.getConsumerKey(), _credentialService.getConsumerSecret());
           IAuthenticationContext authenticationContext = AuthFlow.InitAuthentication(appCreds, _credentialService.getRedirectUrl());
           return authenticationContext.AuthorizationURL;
        }

        public Models.User ValidateTwitterAuth()
        {
            var user = User.GetAuthenticatedUser(_credentialService.GetUserCredentials());

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
            var sanitizer = new HtmlSanitizer();
            var sanitized = sanitizer.Sanitize(newTweet);
            var user = User.GetAuthenticatedUser(_credentialService.GetUserCredentials());
            var tweet = user.PublishTweet(sanitized);
            return !tweet.Text.IsNullOrEmpty() ? sanitized : "An error has occured!";
        }

        public List<Models.Trend> GetTrends()
        {
            var modelTrend = new List<Models.Trend>();

            var result = Auth.ExecuteOperationWithCredentials(_credentialService.GetUserCredentials(), () =>
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
            var sanitizer = new HtmlSanitizer();
            var sanitized = sanitizer.Sanitize(searchTweet);
            var matchingTweets = Search.SearchTweets(sanitized);
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
            var user = User.GetAuthenticatedUser(_credentialService.GetUserCredentials());
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
            var result = Auth.ExecuteOperationWithCredentials(_credentialService.GetUserCredentials(), () =>
            {
                // Get the first 250 followers of the user
                var followers = User.GetFollowers(User.GetAuthenticatedUser(_credentialService.GetUserCredentials()).ScreenName);
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
            var success = Auth.ExecuteOperationWithCredentials(_credentialService.GetUserCredentials(), () =>
            {
                ITweet toDelete = Tweet.GetTweet(id);
                try
                {
                    toDelete.Destroy();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
            return success ? "deleted" : "failed to delete";
        }
    }
}
