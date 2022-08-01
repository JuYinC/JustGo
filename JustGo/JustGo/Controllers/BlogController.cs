using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustGo.Controllers
{
    public class BlogController : Controller
    {
        readonly ILogger _logger;
        readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(ILogger<BlogController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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


        [HttpPost]
        public async Task<IActionResult> uploadImage(IEnumerable<IFormFile> image)
        {
            if (image.First() != null)
            {
                foreach(IFormFile file in image)
                {
                    string WebRootPatch = _webHostEnvironment.WebRootPath;
                    string ProjectPath = _webHostEnvironment.ContentRootPath;
                    string SourcFilename = Path.GetFileName(file.FileName);
                    string TargetFilename = Path.Combine(WebRootPatch,"Uploads",SourcFilename);
                    using(FileStream stream = new FileStream(TargetFilename, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return Json("ok");
        }
    }
}
