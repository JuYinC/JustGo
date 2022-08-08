using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JustGo.Repository;
using System.Drawing;
using System.Drawing.Imaging;

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
        
        [HttpPost]
        [Authorize]
        public IActionResult creatBlog([FromBody]ScheduleVM vm)
        {            
            return Json(_blog.createScheduleToBlog(vm.ScheduleId));
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


        void seveImage(string base64string)
        {
            string[] imageString=base64string.Split(',');
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(imageString[1]);
            }
            catch
            {
                bytes = null;
                Console.WriteLine("sss");
            }
            Image image;
            Random random = new Random();
            string imageName = DateTime.Now.ToString("yyMMdHHmmss") + random.Next(1000, 10000).ToString() + ".png";
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            string WebRootPatch = _webHostEnvironment.WebRootPath;
            string TargetFilename = Path.Combine(WebRootPatch, "Uploads", imageName);
            image.Save(TargetFilename, ImageFormat.Png);
        }
    }
}
