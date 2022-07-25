using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JustGo.Controllers
{
    public class ScheduleController : Controller
    {
        readonly IPlaceWeatherRepostiory _place;
        readonly IScheduleRepostioy _schedule;
        public ScheduleController(IScheduleRepostioy schedule, IPlaceWeatherRepostiory place)
        {
            _place = place;
            _schedule = schedule;
        }
        public IActionResult Index()
        {
            return View();
        }    
        
        public IActionResult getPlace()
        {
            return Json(_place.getPlace(5000,10));
        }


        //--------------------------------------------------
        public IActionResult selectUserSchedule()
        {
            return Json(_schedule.selectUserSchedule(1));
        }

        [HttpPost]
        public IActionResult selectDetail([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.selectScedule(vm.ScheduleId));
        }

        [HttpPost]
        public IActionResult createSchedule([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.createScedule(vm));
        }

        [HttpPost]
        public IActionResult deleteSchedule([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.deleteScedule(vm.ScheduleId));
        }

        [HttpPost]
        public IActionResult editSchedule([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.editScedule(vm));
        }
    }
}
