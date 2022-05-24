using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Display(Name = "Password")]
        //[MinLength(10, ErrorMessage = "Minimum Length Should be 8 Character")]
        public string HashPassword { get; set; } = null!;
        public string ReturnUrl { get; set; } = "/";
        public bool RememberLogin { get; set; } = false;
    }
}
