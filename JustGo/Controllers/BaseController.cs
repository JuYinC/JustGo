using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class BaseController : Controller
    {
        protected ILogger? _logger;

        [Authorize]
        protected string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                _logger?.LogError("Unable to get user ID from claims");
                throw new UnauthorizedAccessException("使用者身份驗證失敗");
            }

            return userId;
        }
    }
}
