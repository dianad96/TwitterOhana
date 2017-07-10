using System;
using Microsoft.AspNetCore.Mvc;
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

        public ActionResult ValidateTwitterAuth()
        {
            var user = _tweetinviService.ValideTwitterAuth(Request);
            ViewData["Name"] = user.Name;
            ViewData["Number_of_followers"] = user.FollowersCount;
            ViewData["Number_of_likes"] = user.FavouritesCount;
            ViewData["Number_of_tweets"] = user.StatusesCount;
          
            return View();
        }

        public string SendTweet(String newTweet)
        {
            var result = _tweetinviService.SendTweet(newTweet);
            return result;
        }

        public IActionResult SearchTweet(String searchTweet)
        {
            var model = _tweetinviService.SearchTweet(searchTweet);
            return View(model);
        }

        public IActionResult GetUserTweets()
        {
            var model = _tweetinviService.GetUserTweets();
            return View(model);
        }

        public String DeleteTweet(long id)
        {
            var result = _tweetinviService.DeleteTweet(id);
            return result;
        }

        public IActionResult GetUserFollowers()
        {
            var model = _tweetinviService.GetUserFollowers();
            return View(model);
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
