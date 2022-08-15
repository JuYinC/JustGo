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
#pragma warning disable CS8600 
            var user = (ClaimsIdentity)User.Identity;
#pragma warning restore CS8600 
#pragma warning disable CS8602 
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
#pragma warning restore CS8602 
            return userId.ToString();
        }
    }
}
