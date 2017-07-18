using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterOhana.Services;

namespace TwitterOhana.Controllers
{

    public class HomeController : Controller
    {
        private readonly ITweetinviService _tweetinviService;

        public HomeController(ITweetinviService tweetinviService)
        {
            _tweetinviService = tweetinviService;
        }

        [HttpGet]
        public ActionResult TwitterAuth()
        {
            return new RedirectResult(_tweetinviService.TwitterAuth());
        }

        public async Task<ActionResult> ValidateTwitterAuth(string oauth_verifier, string authorization_id)
        {
            ITwitterCredentials userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(oauth_verifier, authorization_id);
            await SetAuthCookie(userCredentials);
            return View(_tweetinviService.ValidateTwitterAuth());
        }

        [HttpPost]
        public async Task SetAuthCookie(ITwitterCredentials userCredentials)
        {
            var claims = new List<Claim>
            {
                new Claim("AccessToken", userCredentials.AccessToken),
                new Claim("AccessTokenSecret", userCredentials.AccessTokenSecret)
            };
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
            await HttpContext.Authentication.SignInAsync("CookieAuthentication", HttpContext.User);
        }

        [Authorize]
        public ViewResult GetTrends()
        {
            return View(_tweetinviService.GetTrends());
        }

        [Authorize]
        public string SendTweet(string newTweet)
        {
            return _tweetinviService.SendTweet(newTweet);
        }

        [Authorize]
        public ViewResult SearchTweet(string searchTweet)
        {
            return View(_tweetinviService.SearchTweet(searchTweet));
        }

        [Authorize]
        public ViewResult GetUserTweets()
        {
            return View(_tweetinviService.GetUserTweets());
        }

        [Authorize]
        public string DeleteTweet(long id)
        {
            return _tweetinviService.DeleteTweet(id);
        }

        [Authorize]
        public ViewResult GetUserFollowers()
        {
            return View(_tweetinviService.GetUserFollowers());
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ViewResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public ViewResult Error()
        {
            return View();
        }
    }
}
