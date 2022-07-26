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
        ScheduleVM? testScedule;

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr, IScheduleRepostioy schedule)
        {
            _logger = logger;
            _pwr = pwr;
            _schedule = schedule;
            //testCreateSedule();
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
            Console.WriteLine(select.selectCounty.Length);
            Console.WriteLine(select.selectAcitivity.Length);
            return Json("hi");
        }
        
        public IActionResult teatMapData()
        {
            return Json(_pwr.getPlace(5000, 5));
        }

        public IActionResult testGetShedule()
        {            
                 
            return Json(_schedule.selectScedule(1));
        }
        [HttpPost]
        public IActionResult testSetShedule([FromBody] ScheduleVM vm)
        {            
            vm.Details = new List<ScheduleDetailVM>()
            {
                new ScheduleDetailVM(){
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Place = new Place(){
                        PlaceId=11000,
                        Town = "礁溪鄉",
                    }
                },
                new ScheduleDetailVM(){
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Place = new Place(){
                        PlaceId=11001,
                        Town = "礁溪鄉",
                    }
                },
            };
            _schedule.editScedule(vm);
            return Json("HI");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        
        
    }

}