using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JustGo.Controllers
{
    public class LoginController : Controller
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
        
        public async Task<IActionResult> Login(InputModel Input)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password,Input.RememberMe, lockoutOnFailure: false);
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

    public class InputModel
    {        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Display(Name = "記住密碼?")]
        public bool RememberMe { get; set; }
    }
}
