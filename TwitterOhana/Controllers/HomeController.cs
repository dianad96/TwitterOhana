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

            HttpContext.Session.SetString("AuthorizationKey", _authenticationContext.Token.AuthorizationKey);
            HttpContext.Session.SetString("AuthorizationSecret", _authenticationContext.Token.AuthorizationSecret);

            return new RedirectResult(_authenticationContext.AuthorizationURL);
        }

        public ActionResult ValidateTwitterAuth()
        {
            HttpContext.Session.SetString("verifierCode", Request.Query.ElementAt(2).Value);
            HttpContext.Session.SetString("verifierSecret", Request.Query.ElementAt(1).Value);
            HttpContext.Session.SetString("authorizationId", Request.Query.ElementAt(0).Value);

            if (!HttpContext.Session.GetString("verifierCode").IsNullOrEmpty())
            {
                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(HttpContext.Session.GetString("verifierCode"), HttpContext.Session.GetString("authorizationId"));
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

            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(HttpContext.Session.GetString("verifierCode"), HttpContext.Session.GetString("authorizationId"));
            var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);

            var tweet = user.PublishTweet(NewTweet);

            if (!tweet.Text.IsNullOrEmpty())
                return "Sent!";
            else
                return "An error has occured!";
        }

        public IActionResult searchTweet(String SearchTweet)
        {
            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(HttpContext.Session.GetString("verifierCode"), HttpContext.Session.GetString("authorizationId"));

            var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);
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
            var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(HttpContext.Session.GetString("verifierCode"), HttpContext.Session.GetString("authorizationId"));
            var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);
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
                    Id = e.Current.Id
            });
            }
            return View(model);
        }

        public String deleteTweet()
        {

            return "deleted";
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
