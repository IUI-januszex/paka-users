using System.ComponentModel.DataAnnotations;
namespace PakaUsers.Controllers
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Address is required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}