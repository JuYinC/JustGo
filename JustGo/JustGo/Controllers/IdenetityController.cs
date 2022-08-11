using JustGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JustGo.Controllers
{
    public class LoginController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
       
        public IActionResult index()
        {
            return View("Login");
        }
        
        public async Task<IActionResult> Login(UserVM vm)
        {
            var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, lockoutOnFailure: false);
            string returnUrl = Url.Content("~/");
            if (result.Succeeded)
            {                
                return LocalRedirect(returnUrl);
            }
            return View("Login");
        }

        public async Task<IActionResult> Logout()
        {
            string returnUrl = Url.Content("~/");
            await _signInManager.SignOutAsync();            
            return LocalRedirect(returnUrl);            
        }
    }    
}
