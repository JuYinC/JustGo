using JustGo.Repository;
using Microsoft.AspNetCore.Mvc;
using JustGo.ViewModels;

namespace JustGo.Controllers
{
    public class SearchController : Controller
    {
        private readonly IScheduleRepostioy _schedule;
        public SearchController(IScheduleRepostioy schedule)
        {
            _schedule = schedule;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult testSearch([FromBody]SelectPlaceVM vm)
        {
            Console.WriteLine(vm.selectCounty.Length);
            Console.WriteLine(vm.selectAcitivity.Length);
            return Json(_schedule.selectUserSchedule("862c02ac-67e1-461f-ac15-74b166c0a1e4"));
        }
    }
}
