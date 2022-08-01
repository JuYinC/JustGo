using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class ScheduleController : Controller
    {
        readonly IPlaceWeatherRepostiory _place;
        readonly IScheduleRepostioy _schedule;
        readonly ILogger _logger;
        public ScheduleController(IScheduleRepostioy schedule, IPlaceWeatherRepostiory place, ILogger<ScheduleController> logger)
        {
            _place = place;
            _schedule = schedule;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult getPlace([FromBody] SelectPlaceVM select)
        {            
            return Json(_place.getPlace(select));
        }

        [HttpPost]
        public IActionResult selectPlaceFilter([FromBody]SelectPlaceVM vm)
        {            
            return Json(_place.getPlaceFilter(vm));
        }

        //--------------------------------------------------
        //搜尋使用者行程清單/無細項
        public IActionResult selectUserSchedule()
        {
            Console.WriteLine(Json(_schedule.selectUserSchedule(GetUserId())));
            //return Json(_schedule.selectUserSchedule("1"));
            return Json(_schedule.selectUserSchedule(GetUserId()));
            //return Json("123");
        }

        //搜尋行程細項
        [HttpPost]
        public IActionResult selectDetail([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.selectScedule(vm.ScheduleId, GetUserId()));
        }

        //新增行程
        [HttpPost]
        [Authorize]
        public IActionResult setSchedule([FromBody] ScheduleVM vm)
        {
            if (vm.UserId != null)
            {
                vm.UserId = GetUserId();
                return Json(_schedule.editScedule(vm));
            }
            vm.UserId = GetUserId();
            return Json(_schedule.createScedule(vm));
            //return View("Index");

        }

        //刪除行程
        [HttpPost]
        public IActionResult deleteSchedule([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.deleteScedule(vm.ScheduleId));
        }

        string GetUserId()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId.ToString();
        }
    }
}
