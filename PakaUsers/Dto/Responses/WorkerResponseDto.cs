using System;
using System.Collections.Generic;
using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class WorkerResponseDto : UserResponseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Salary { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseType { get; set; }

        public static WorkerResponseDto Of(Worker user)
        {
            return new WorkerResponseDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserType = user.UserType,
                Name = user.Name,
                Surname = user.Surname,
                Salary = user.Salary,
                WarehouseId = user.WarehouseId
            };
        }
    }
}