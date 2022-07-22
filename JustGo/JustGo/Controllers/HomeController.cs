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
        private readonly IScheduleRepostioy _schedule;
        Schedule testScedule;

        public HomeController(ILogger<HomeController> logger, IPlaceWeatherRepostiory pwr, IScheduleRepostioy schedule)
        {
            _logger = logger;
            _pwr = pwr;
            _schedule = schedule;
            //testUpdataSedule();
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

        //void testCreateSedule()
        //{
        //    testScedule = new Schedule()
        //    {
        //        UserId = 1,
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now,
        //        WeatherWarning = false,                
        //    };
        //    List<ScheduleDetails> scheduleDetails = new List<ScheduleDetails>()
        //    {
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 22,Town="前金區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 23,Town="左營區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 24,Town="鳳山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 25,Town="岡山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 26,Town="小港區"},
        //    };
        //    _schedule.createScedule(testScedule,scheduleDetails);
        //}
        //void testUpdataSedule()
        //{
        //    List<ScheduleDetails> testListSedule = new List<ScheduleDetails>()
        //     {
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 12,Town="岡山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 55,Town="岡山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 15,Town="岡山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 135,Town="岡山區"},
        //            new ScheduleDetails(){StartTime=DateTime.Now,EndtTime=DateTime.Now,PlaceId = 1026,Town="岡山區"}
        //     };
        //    testScedule = _schedule.selectUserSchedule(1).ToList()[0];
            
        //    _schedule.editScedule(testScedule,testListSedule);

        //}
    }
}