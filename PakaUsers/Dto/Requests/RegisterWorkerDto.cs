using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class RegisterWorkerDto : RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Warehouse is required")]
        public int Warehouse { get; set; }
        [Required(ErrorMessage = "Salary is required")]
        public decimal Salary { get; set; }
    }
}