using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JustGo.Repository;

namespace JustGo.Controllers
{
    public class BlogController : BaseController
    {
        readonly ILogger _logger;
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly IBlogRepostioy _blog;
        public BlogController(ILogger<BlogController> logger, IWebHostEnvironment webHostEnvironment,IBlogRepostioy blog)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _blog = blog;
        }

        [HttpPost]
        [Authorize]
        public IActionResult setBlog([FromBody] BlogVM vm)
        {
            if(vm != null)
            {
                if(vm.BlogId != 0)
                {
                    vm.UserId = GetUserId();
                    return Json(_blog.editBlog(vm));
                }
                vm.UserId = GetUserId();
                return Json(_blog.createBlog(vm));
            }
            return Json(vm);
        }

        //傳圖 (測試)
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

        //查詢使用者部落格(無細項)
        [HttpPost]
        [Authorize]
        public IActionResult selectUserBlog()
        {
            return Json(_blog.selectUserBlog(GetUserId()));
        }

        //Blog細項
        [HttpPost]
        public IActionResult selectblogDetails([FromBody] BlogVM vm)
        {
            return Json(_blog.selectBlog(vm.BlogId));
        }

        //搜尋部落格
        public IActionResult searchBlog([FromBody] SelectPlaceVM vm)
        {
            return Json(_blog.getBlogFilter(vm));
        }
    }
}
