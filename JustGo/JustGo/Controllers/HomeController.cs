using JustGo.Models;
using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace JustGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlaceWeatherRepostiory _pwr;
        private readonly IScheduleRepostioy _schedule;
        private readonly IBlogRepostioy _blog;

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr, IScheduleRepostioy schedule,IBlogRepostioy blog)
        {
            _logger = logger;
            _pwr = pwr;
            _schedule = schedule;           
            _blog = blog;
        }
        public IActionResult Index()
        {
            return View("Blog");
        }

        public IActionResult Index0728()
        {
            return View();
        }
        public IActionResult block1()
        {
            return View();
    }
    public IActionResult blog()
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

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult selectPlace([FromBody]SelectPlaceVM select)
        {
            return Json(_pwr.getPlaceFilter(select));
        }

        [HttpPost]
        public IActionResult teatMapData([FromBody] SelectPlaceVM select)
        {
            SelectPlaceVM selectPlaceVM = new SelectPlaceVM() { Lat = 22.6397082860113, Lng = 120.30264837097221 };
            return Json(_pwr.getPlace(select));            
        }
            

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        
        
    }

}