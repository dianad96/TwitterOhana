using System;
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
        private String redirectURL = "http://localhost:2559/";
        public HomeController(ITweetinviService tweetinviService)
        {
            _tweetinviService = tweetinviService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult TwitterAuth()
        {
            return new RedirectResult(_tweetinviService.TwitterAuth());
        }

        [AllowAnonymous]
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
                new Claim("AccessTokenSecret", userCredentials.AccessTokenSecret),
                new Claim("IsAuthenticated", "true")
            };
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
            await HttpContext.Authentication.SignInAsync("CookieAuthentication", HttpContext.User);
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult GetTrends()
        {
            return View(_tweetinviService.GetTrends());
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public string SendTweet(string newTweet)
        {
            return _tweetinviService.SendTweet(newTweet);
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult SearchTweet(string searchTweet)
        {
            return View(_tweetinviService.SearchTweet(searchTweet));
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult GetUserTweets()
        {
            return View(_tweetinviService.GetUserTweets());
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public string DeleteTweet(long id)
        {
            return _tweetinviService.DeleteTweet(id);
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult GetUserFollowers()
        {
            return View(_tweetinviService.GetUserFollowers());
        }

        [AllowAnonymous]
        public async Task<ViewResult> Index()
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");
            return View();
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult About()
        {
            return View();
        }

        [Authorize(Policy = "TwitterAuthenticated")]
        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Error()
        {
            return View();
        }

        public ViewResult Forbidden()
        {
            return View();
        }
    }
}
