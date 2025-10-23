using JustGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JustGo.Repository;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using JustGo.Constants;

namespace JustGo.Controllers
{
    public class BlogController : BaseController
    {
        new readonly ILogger _logger;
        readonly IWebHostEnvironment _webHostEnvironment;        
        readonly IUnitOfWork _unit;
        public BlogController(ILogger<BlogController> logger, IWebHostEnvironment webHostEnvironment,IUnitOfWork unit)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;            
            _unit = unit;
        }

        [HttpPost]
        [Authorize]
        public IActionResult setBlog([FromBody] BlogVM vm)
        {
            //return Json(true);

            if (vm != null)
            {
                if (vm.Details != null)
                {
                    if (vm.CoverImage != null)
                    {
                        saveImage(vm.CoverImage);
                    }
                    foreach (var day in vm.Details)
                    {
                        foreach (var item in day)
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
                    if (vm.BlogId != 0)
                    {
                        vm.UserId = GetUserId();
                        return Json(_unit.blog.editBlog(vm));
                    }
                    vm.UserId = GetUserId();
                    return Json(_unit.blog.createBlog(vm));
                }
            }
            return Json(false);
        }

        [HttpPost]
        [Authorize]
        public IActionResult creatBlog([FromBody] ScheduleVM vm)
        {
            return Json(_unit.blog.createScheduleToBlog(vm.ScheduleId,GetUserId()));
        }

        [HttpPost]
        [Authorize]
        public IActionResult deleteBlog([FromBody]BlogVM vm)
        {
            // Security: Verify the blog belongs to the current user
            var userId = GetUserId();
            var existingBlog = _unit.blog.selectBlog(vm.BlogId);

            if (existingBlog == null || existingBlog.BlogId == 0)
            {
                _logger.LogWarning("Attempt to delete non-existent blog. BlogId: {BlogId}", vm.BlogId);
                return NotFound(new { message = "部落格不存在" });
            }

            if (existingBlog.UserId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt. UserId: {UserId}, BlogId: {BlogId}", userId, vm.BlogId);
                return Forbid(); // 403 Forbidden
            }

            var result = _unit.blog.deleteBlog(vm);
            if (result)
            {
                _logger.LogInformation("Blog deleted successfully. BlogId: {BlogId}, UserId: {UserId}", vm.BlogId, userId);
            }

            return Json(result);
        }

        //查詢使用者部落格(無細項)        
        [Authorize]
        public IActionResult selectUserBlog()
        {
            return Json(_unit.blog.selectUserBlog(GetUserId()));
        }

        //Blog細項
        [HttpPost]
        public IActionResult selectblogDetails([FromBody] BlogVM vm)
        {            
            return Json(_unit.blog.selectBlog(vm.BlogId));
        }

        //查詢使用者是否有收藏
        [HttpPost]
        [Authorize]
        public IActionResult getIsKeep([FromBody] UserKeepVM vm)
        {
            vm.KeepClass = 0;
            vm.UserId = GetUserId();
            return Json(_unit.keep.IsKeep(vm));
        }

        //搜尋部落格
        public IActionResult searchBlog([FromBody] SelectPlaceVM vm)
        {            
            return Json(_unit.blog.getBlogFilter(vm));
        }

        [Authorize]
        public IActionResult getUserKeepBlog()
        {
            var vm = new UserKeepVM();
            vm.UserId = GetUserId();
            return Json(_unit.blog.getKeepBlog(vm));
        }

        //加入收藏或移除 回傳bool
        [HttpPost]
        [Authorize]
        public IActionResult userKeepBlog([FromBody] UserKeepVM vm)
        {
            vm.KeepClass = 0;
            vm.UserId = GetUserId();
            return Json(_unit.keep.Keep(vm));
        }

        public IActionResult getBlogTop4()
        {
            return Json(_unit.blog.getBlogRank());
        }

        //圖片儲存
        void saveImage(blogImage blogImage)
        {
            string[] imageString;
            byte[] bytes;

            // Validate base64 is not null
            if (string.IsNullOrEmpty(blogImage.base64))
            {
                _logger.LogWarning("Image base64 is null or empty, skipping save");
                throw new ArgumentException("圖片資料為空");
            }

            try
            {
                imageString = blogImage.base64.Split(",");
                if (imageString.Length < 2)
                {
                    _logger.LogError("Invalid base64 format for image: {ImageName}. Expected format 'data:image/type;base64,data'", blogImage.name);
                    throw new ArgumentException("圖片格式錯誤");
                }
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                _logger.LogError(ex, "Error splitting base64 string for image: {ImageName}", blogImage.name);
                throw new ArgumentException("圖片格式錯誤", ex);
            }

            // Validate MIME type before processing
            var mimeType = imageString[0];
            if (mimeType != AppConstants.FileUpload.MimeTypes.Png &&
                mimeType != AppConstants.FileUpload.MimeTypes.Jpeg)
            {
                _logger.LogWarning("Unsupported image MIME type: {MimeType}", mimeType);
                throw new ArgumentException($"不支援的圖片格式: {mimeType}");
            }

            blogImage.base64 = "";

            try
            {
                bytes = Convert.FromBase64String(imageString[1]);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid base64 data for image: {ImageName}", blogImage.name);
                throw new ArgumentException("圖片資料格式錯誤", ex);
            }

            // Validate file size
            if (bytes.Length > AppConstants.FileUpload.MaxFileSizeBytes)
            {
                _logger.LogWarning("Image file size {FileSize} exceeds maximum allowed size {MaxSize}",
                    bytes.Length, AppConstants.FileUpload.MaxFileSizeBytes);
                throw new ArgumentException($"檔案大小超過限制 ({AppConstants.FileUpload.MaxFileSizeMB}MB)");
            }
            Image image;
            string WebRootPatch = _webHostEnvironment.WebRootPath;
#pragma warning disable CA1416 // 驗證平台相容性
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    // Validate it's actually a valid image by loading it
                    image = Image.FromStream(ms);

                    // Generate secure filename using GUID to prevent predictable filenames
                    if (string.IsNullOrEmpty(blogImage.name))
                    {
                        blogImage.name = Guid.NewGuid().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid image data - could not load image from stream");
                throw new ArgumentException("圖片資料無效", ex);
            }

            string extension = "";
            ImageFormat format;

            switch (imageString[0])
            {
                case "data:image/png;base64":
                    extension = ".png";
                    format = ImageFormat.Png;
                    break;
                case "data:image/jpeg;base64":
                    extension = ".jpg";
                    format = ImageFormat.Jpeg;
                    break;
                default:
                    _logger.LogWarning("Unsupported image format: {Format}", imageString[0]);
                    throw new ArgumentException($"不支援的圖片格式: {imageString[0]}");
            }

            // Validate extension
            if (!AppConstants.FileUpload.AllowedImageExtensions.Contains(extension.ToLower()))
            {
                _logger.LogWarning("File extension {Extension} not in allowed list", extension);
                throw new ArgumentException($"不支援的圖片格式: {extension}");
            }

            blogImage.name += extension;
            string TargetFilename = Path.Combine(WebRootPatch, "blogImages", blogImage.name);

            try
            {
                image.Save(TargetFilename, format);
                _logger.LogInformation("Image saved successfully: {FileName}", TargetFilename);
            }
            catch (Exception ex) when (format == ImageFormat.Jpeg)
            {
                _logger.LogWarning(ex, "Failed to save JPEG directly, converting to Bitmap. File: {FileName}", TargetFilename);
                try
                {
                    var bitmap = new Bitmap(image);
                    bitmap.Save(TargetFilename, ImageFormat.Jpeg);
                    _logger.LogInformation("Successfully saved JPEG after Bitmap conversion. File: {FileName}", TargetFilename);
                }
                catch (Exception bitmapEx)
                {
                    _logger.LogError(bitmapEx, "Failed to save JPEG even after Bitmap conversion. File: {FileName}", TargetFilename);
                    throw;
                }
            }
#pragma warning restore CA1416 // 驗證平台相容性
        }
    }
}
