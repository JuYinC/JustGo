using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        //搜尋使用者行程清單/無細項
        public IActionResult selectUserSchedule()
        {
            return Json(_schedule.selectUserSchedule("1"));
        }

        //搜尋行程細項
        [HttpPost]
        public IActionResult selectDetail([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.selectScedule(vm.ScheduleId));
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
