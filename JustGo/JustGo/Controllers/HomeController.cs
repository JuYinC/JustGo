using JustGo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
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
            Console.WriteLine(1);
            string[] a = new string[] { "a" };
            string[] b = new string[] { "高雄市", "澎湖縣" };
            string[] c = new string[] { "前金區", "馬公市" };
            int[] d = new int[] { 2, 3, 4 };
            IEnumerable list = _pwr.getPlaceFilter(a, b, c, d);
            ViewBag.Json = JsonConvert.SerializeObject(list);
            ////ViewBag.Json = Json(_pwr.getPlaceFilter(a, b, c, d));_pwr.getPlace(10,100)
            return View(list);
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