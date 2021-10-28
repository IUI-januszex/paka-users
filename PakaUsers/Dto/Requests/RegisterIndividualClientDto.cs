using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class RegisterIndividualClientDto : RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
    }
}