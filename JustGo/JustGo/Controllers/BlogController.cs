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
    }
}
