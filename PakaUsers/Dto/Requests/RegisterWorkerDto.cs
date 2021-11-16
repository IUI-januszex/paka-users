using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class RegisterWorkerDto : RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseType { get; set; }
        [Required(ErrorMessage = "Salary is required")]
        public decimal Salary { get; set; }
    }
}