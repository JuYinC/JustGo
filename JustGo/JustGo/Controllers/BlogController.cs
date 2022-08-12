using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JustGo.Repository;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

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
            saveImage(vm.CoverImage);
            foreach (var day in vm.Details)
            {
                foreach(var item in day)
                {
                    if (item.Images != null)
                    {
                        for (int i = 0; i < item.Images.Count; i++)
                        {                                          
                            saveImage(item.Images[i]);                            
                        }
                    }                    
                }
            }
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
        public IActionResult selectblogDetails([FromBody]BlogVM vm)
        {
          
            return Json(_blog.selectBlog(vm.BlogId));
        }

        //搜尋部落格
        public IActionResult searchBlog([FromBody] SelectPlaceVM vm)
        {
            return Json(_blog.getBlogFilter(vm));
        }


        void saveImage(blogImage blogImage)
        {
            string[] imageString;
            Console.WriteLine(blogImage.base64);
            try
            {
                imageString = blogImage.base64.Split(",");
            }
            catch
            {
                return;
            }
            blogImage.base64 = "";            
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(imageString[1]);
            }
            catch
            {                
                return;
            }
            Image image;            
            string WebRootPatch = _webHostEnvironment.WebRootPath;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                if (blogImage.name == "")
                {
                    Random random = new Random();
                    blogImage.name = DateTime.Now.ToString("yyMMdHHmmss") + random.Next(1000, 10000).ToString();
                }
            }
            string TargetFilename = Path.Combine(WebRootPatch, "blogImages", blogImage.name);
            switch (imageString[0]) {
                case "data:image/png;base64":
                    TargetFilename += ".png";
                    blogImage.name += ".png";
                    image.Save(TargetFilename, ImageFormat.Png);
                    break;
                case "data:image/jpeg;base64":
                    TargetFilename += ".jpg";
                    blogImage.name +=".jpg";
                    try
                    {
                        image.Save(TargetFilename, ImageFormat.Jpeg);
                    }
                    catch
                    {
                        var i = new Bitmap(image);
                        i.Save(TargetFilename, ImageFormat.Jpeg);
                    }                    
                    break;
                //case "data:image/gif;base64":
                //    TargetFilename += ".gif";
                //    blogImage.name += ".gif";
                //    image.Save(TargetFilename, ImageFormat.Gif);
                //    break;
                default:
                    return;                    
            }                                    
        }
    }
}
