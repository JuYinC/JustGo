using JustGo.Models;
using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlaceWeatherRepostiory _pwr;
        private readonly IScheduleRepostioy _schedule;
        private readonly IBlogRepostioy _blog;

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr, IScheduleRepostioy schedule, IBlogRepostioy blog)
        {
            _logger = logger;
            _pwr = pwr;
            _schedule = schedule;
            _blog = blog;           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View("EditBlog");
        }

        public IActionResult EditBlog()
        {
            return View();
        }

        public IActionResult itinerary()
        {
            return View();
        }

        public IActionResult UserCatelog()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}