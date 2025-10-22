using JustGo.Models;
using JustGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace JustGo.Controllers
{
    public class LoginController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<LoginController> _logger;

        public LoginController(UserManager<ApplicationUser> userManager,IUserStore<ApplicationUser> userStore, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult index()
        {
            return View("Login");
        }

        public async Task<IActionResult> Register(RegisterVM vm)
        {
            string returnUrl = Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Name = vm.Name;
                user.City = vm.City ?? "";

                await _userStore.SetUserNameAsync(user, vm.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, vm.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Skip email confirmation for now and sign in directly
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User logged in after registration.");
                    return LocalRedirect(returnUrl);
                }

                // Add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Return to login view with errors
            return View("Login", new UserVM());
        }

        public async Task<IActionResult> Login(UserVM vm)
        {
            string returnUrl = Url.Content("~/");

            // Check if email and password are provided
            if (string.IsNullOrEmpty(vm?.Email) || string.IsNullOrEmpty(vm?.Password))
            {
                ModelState.AddModelError(string.Empty, "請輸入信箱和密碼 (Please enter email and password)");
                return View("Login", vm);
            }

            if (ModelState.IsValid)
            {
                // Find user by email first, then use their username for sign-in
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, vm.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        ModelState.AddModelError(string.Empty, "此帳號已被鎖定，請稍後再試 (Account locked, try again later)");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "登入失敗，請檢查信箱和密碼 (Invalid email or password)");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "登入失敗，請檢查信箱和密碼 (Invalid email or password)");
                }
            }

            return View("Login", vm);
        }

        public async Task<IActionResult> Logout()
        {
            string returnUrl = Url.Content("~/");
            await _signInManager.SignOutAsync();            
            return LocalRedirect(returnUrl);            
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }    
}
