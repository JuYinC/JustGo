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

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr, IScheduleRepostioy schedule)
        {
            _logger = logger;
            _pwr = pwr;
            _schedule = schedule;
            
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
        public IActionResult selectPlace([FromBody]SelectPlaceVM select)
        {
            return Json(_pwr.getPlaceFilter(select));
        }
        
        public IActionResult teatMapData()
        {
            return Json(_pwr.getPlace(5000, 5));
        }

        public IActionResult testGetShedule()
        {            
                 
            return Json(_schedule.selectScedule(1));
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        
        
    }

}