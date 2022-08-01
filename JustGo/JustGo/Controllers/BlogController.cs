using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustGo.Controllers
{
    public class BlogController : Controller
    {
        readonly ILogger _logger;
        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[Authorize]
        public IActionResult setBlog(BlogVM vm)
        {
            Console.WriteLine(vm.Title);
            return Json(vm);
        }
    }
}
