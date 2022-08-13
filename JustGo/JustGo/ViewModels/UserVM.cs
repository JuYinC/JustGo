using System.ComponentModel.DataAnnotations;

namespace JustGo.ViewModels
{
    public class UserVM
    {        
        public string? Email { get; set; }

        public string? Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
