using JustGo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult axiosTest(string request)
        {
            if (request == null)
            {
                return Error();
            }
            Request jObj = JsonConvert.DeserializeObject<Request>(request)??new Request();
            if (jObj.start < 1 ||jObj.quantity<1)
            {
                return Error();
            }
            Console.WriteLine(jObj.start);
            Console.WriteLine(jObj.quantity);
            return Json(_pwr.getPlace(jObj.start, jObj.quantity));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
    public class Request {
        public int start { get; set;}
        public int quantity { get; set;}
    }

}