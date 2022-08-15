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

                user.City = vm.City;

                await _userStore.SetUserNameAsync(user, vm.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, vm.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(vm.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = vm.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
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
