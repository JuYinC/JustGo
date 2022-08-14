using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace JustGo.Controllers
{
    public class BaseController : Controller
    {
        [Authorize]
        protected string GetUserId()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId.ToString();
        }
    }
}
