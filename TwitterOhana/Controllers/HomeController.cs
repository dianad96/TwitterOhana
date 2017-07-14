using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var model = _tweetinviService.ValidateTwitterAuth(oauth_verifier, authorization_id);
            return View(model);
        }

        public ViewResult GetTrends()
        {
            var model = _tweetinviService.GetTrends();
            return View(model);
        }

        public string SendTweet(string newTweet)
        {
            return _tweetinviService.SendTweet(newTweet);
        }

        public ViewResult SearchTweet(string searchTweet)
        {
            var model = _tweetinviService.SearchTweet(searchTweet);
            return View(model);
        }

        public ViewResult GetUserTweets()
        {
            var model = _tweetinviService.GetUserTweets();
            return View(model);
        }

        public string DeleteTweet(long id)
        {
            return _tweetinviService.DeleteTweet(id);
        }

        public ViewResult GetUserFollowers()
        {
            var model = _tweetinviService.GetUserFollowers();
            return View(model);
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
