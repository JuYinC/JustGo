﻿using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class ScheduleController : BaseController
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

        [HttpPost]
        [Authorize]
        public IActionResult copySchedule(DateTime time,int blogId)
        {
            return Json(_schedule.copyScheduleByBlog(time, blogId,GetUserId()));
        }
        //--------------------------------------------------
        //搜尋使用者行程清單/無細項
        [Authorize]
        public IActionResult selectUserSchedule()
        {
            return Json(_schedule.selectUserSchedule(GetUserId()));
        }

        //搜尋行程細項
        [HttpPost]
        [Authorize]
        public IActionResult selectDetail([FromBody] ScheduleVM vm)
        {
            return Json(_schedule.selectScedule(vm.ScheduleId,GetUserId()));
        }

        //新增行程 修改行程
        [HttpPost]
        [Authorize]
        public IActionResult setSchedule([FromBody] ScheduleVM vm)
        {
            if (vm.ScheduleId != 0)
            {
                vm.UserId = GetUserId();
                return Json(_schedule.editScedule(vm));
            }
            vm.UserId = GetUserId();
            return Json(_schedule.createScedule(vm));            
        }

        //刪除行程
        [HttpPost]
        [Authorize]
        public IActionResult deleteSchedule([FromBody] ScheduleVM vm)
        {
            vm.UserId = GetUserId();
            if(_schedule.deleteScedule(vm))
            {
                return Json(vm.ScheduleId);
            }
            return Json(0);
        }

        public IActionResult selectWeather()
        {
            return Json(_place.getWeatherByLocation("dd"));
        }
    }
}
