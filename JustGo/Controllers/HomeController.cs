using JustGo.Models;
using JustGo.Repository;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;                        
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult EditBlog()
        {
            return View();
        }

        [Authorize]
        public IActionResult itinerary()
        {
            return View();
        }

        public IActionResult UserCatelog()
        {
            return View();
        }
        public IActionResult check()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}