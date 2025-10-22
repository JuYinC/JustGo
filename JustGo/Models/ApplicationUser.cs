using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JustGo.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? City { get; set; }
    }
}
