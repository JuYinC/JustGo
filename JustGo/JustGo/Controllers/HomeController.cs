using JustGo.Models;
using JustGo.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace JustGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlaceWeatherRepostiory _pwr;

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr)
        {
            _logger = logger;
            _pwr = pwr;
            
        }

        public IActionResult Index()
        {
            ViewBag.Json = "123";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult teatMapData()
        {
            return Json(_pwr.getPlace(5000,5));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}