using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class ScheduleController : BaseController
    {
        
        readonly ILogger _logger;
        readonly IUnitOfWork _unit;
        public ScheduleController(ILogger<ScheduleController> logger, IUnitOfWork unit)
        {
            
            _logger = logger;
            _unit = unit;
        }

        [HttpPost]
        public IActionResult getPlace([FromBody] SelectPlaceVM select)
        {            
            return Json(_unit.place.getPlace(select));
        }

        [HttpPost]
        public IActionResult selectPlaceFilter([FromBody]SelectPlaceVM vm)
        {
            vm.selectType = (vm.selectType=="") ? "景點" : vm.selectType;
            if ((vm.selectCounty!=null && vm.selectCounty.Length > 0) || (vm.selectAcitivity != null && vm.selectAcitivity.Length > 0) || vm.selectType != "景點")
            {
                return Json(_unit.place.getPlaceFilter(vm));
            }
            else
            {
                return Json(_unit.place.getPlace(vm));
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult copySchedule([FromBody]BlogVM vm)
        {
            vm.UserId = GetUserId();
            if (_unit.schedule.copyScheduleByBlog(vm))
            {
                return Json("複製成功");
            }
            return Json("複製失敗");
        }
        //--------------------------------------------------
        //搜尋使用者行程清單/無細項
        [Authorize]
        public IActionResult selectUserSchedule()
        {
            return Json(_unit.schedule.selectUserSchedule(GetUserId()));
        }

        //搜尋行程細項
        [HttpPost]
        [Authorize]
        public IActionResult selectDetail([FromBody] ScheduleVM vm)
        {
            return Json(_unit.schedule.selectScedule(vm.ScheduleId,GetUserId()));
        }

        //新增行程 修改行程
        [HttpPost]
        [Authorize]
        public IActionResult setSchedule([FromBody] ScheduleVM vm)
        {
            if (vm.ScheduleId != 0)
            {
                vm.UserId = GetUserId();
                return Json(_unit.schedule.editScedule(vm));
            }
            vm.UserId = GetUserId();
            return Json(_unit.schedule.createScedule(vm));            
        }

        //刪除行程
        [HttpPost]
        [Authorize]
        public IActionResult deleteSchedule([FromBody] ScheduleVM vm)
        {
            vm.UserId = GetUserId();
            if(_unit.schedule.deleteScedule(vm))
            {
                return Json(vm.ScheduleId);
            }
            return Json(0);
        }
        

        public IActionResult selectWeather()
        {
            return Json(_unit.place.getWeatherByLocation("dd"));
        }
    }
}
