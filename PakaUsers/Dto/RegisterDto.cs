using System.ComponentModel.DataAnnotations;
using PakaUsers.Model;

namespace PakaUsers.Controllers
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "User Type is required")]
        public UserType UserType { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}