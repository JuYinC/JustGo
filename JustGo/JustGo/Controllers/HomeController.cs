using JustGo.Models;
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
            string[] a = new string[] { "a" };
            string[] b = new string[] {"高雄市","澎湖縣"};
            string[] c = new string[] {};
            int[] d = new int[] { 1,2 };
            ViewBag.Json = JsonConvert.SerializeObject(_pwr.getPlaceFilter(a , b, c, d));
            //ViewBag.Json = Json(_pwr.getPlaceFilter(a, b, c, d));
            return View();
        }

        public IActionResult Privacy()
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