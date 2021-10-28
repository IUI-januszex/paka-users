using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class RegisterBusinessClientDto : RegisterDto
    {
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "NIP is required")]
        public string Nip { get; set; }
    }
}