using System.ComponentModel.DataAnnotations;

namespace JustGo.ViewModels
{
    public class UserVM
    {        
        public string? Email { get; set; }

        public string? Password { get; set; }
        
        public bool RememberMe { get; set; }
    }

    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string? Name { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }
       
        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
        public string? ConfirmPassword { get; set; }
    }
}
