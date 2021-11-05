using System;

namespace PakaUsers.Dto.Responses
{
    public class WorkerResponseDto : UserResponseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Salary { get; set; }
        public int WarehouseId { get; set; }
    }
}