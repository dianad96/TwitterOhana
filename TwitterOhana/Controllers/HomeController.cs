using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using TwitterOhana.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TwitterOhana.Controllers
{

    public class HomeController : Controller
    {
        private IAuthenticationContext _authenticationContext;
        
        [HttpGet]
        public ActionResult TwitterAuth()
        {
            var appCreds = new ConsumerCredentials(MyCredentials.CONSUMER_KEY, MyCredentials.CONSUMER_SECRET);

            _authenticationContext = AuthFlow.InitAuthentication(appCreds, MyCredentials.redirectURL);

            return new RedirectResult(_authenticationContext.AuthorizationURL);
        }

        public ActionResult ValidateTwitterAuth()
        {
            if (!Request.Query.ElementAt(2).Value.IsNullOrEmpty())
            {
                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(Request.Query.ElementAt(2).Value, Request.Query.ElementAt(0).Value);
                MyCredentials.myCreds = userCreds;
                var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);
                   
                ViewData["Name"] = user.Name;
                ViewData["Number_of_followers"] = user.FollowersCount;
                ViewData["Number_of_likes"] = user.ListedCount;
                ViewData["Number_of_tweets"] = user.StatusesCount;
            }
            return View();
        }

        public String sendTweet(String NewTweet)
        {
            var user = Tweetinvi.User.GetAuthenticatedUser(MyCredentials.myCreds);
            var tweet = user.PublishTweet(NewTweet);
            if (!tweet.Text.IsNullOrEmpty())
                return "Sent!";
            else
                return "An error has occured!";
        }

        public IActionResult searchTweet(String SearchTweet)
        {
            var user = Tweetinvi.User.GetAuthenticatedUser(MyCredentials.myCreds);
            var matchingTweets = Search.SearchTweets(SearchTweet);

            var model = new List<TwitterOhana.Models.Tweet>();

            IEnumerator<Tweetinvi.Models.ITweet> e = matchingTweets.GetEnumerator();
            while (e.MoveNext())
            {
                model.Add(new TwitterOhana.Models.Tweet
                {
                    Text=e.Current.Text,
                    UserName = e.Current.CreatedBy.Name,
                    ScreenName = e.Current.CreatedBy.ScreenName,
                    CreatedAt = e.Current.CreatedAt,
                    UserProfileImage = e.Current.CreatedBy.ProfileImageUrl
                });
              
                String value = e.Current.Text;
                Console.WriteLine(value);
            }
            return View(model);
        }

        public IActionResult getUserTweets()
        {
            var user = Tweetinvi.User.GetAuthenticatedUser(MyCredentials.myCreds);
            var userTweets = user.GetHomeTimeline();

            var model = new List<TwitterOhana.Models.Tweet>();

            IEnumerator<Tweetinvi.Models.ITweet> e = userTweets.GetEnumerator();
            while (e.MoveNext())
            {
                model.Add(new TwitterOhana.Models.Tweet
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
            return View(model);
        }

        public String deleteTweet(long Id)
        {
            var user = Tweetinvi.User.GetAuthenticatedUser(MyCredentials.myCreds);

            var success = Auth.ExecuteOperationWithCredentials(MyCredentials.myCreds, () =>
            {
                ITweet toDelete = Tweetinvi.Tweet.GetTweet(Id);
                var tweet = toDelete.Destroy();

                return tweet;
            });

            if (success)
                return "deleted";
            else
                return "fail";
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
