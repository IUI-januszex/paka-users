using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }

        public static UserResponseDto Of(User user)
        {
            switch (user.UserType)
            {
                case UserType.Logistician:
                    return new WorkerResponseDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                        UserType = user.UserType,
                        Name = (user as Logistician)?.Name,
                        Surname = (user as Logistician)?.Surname,
                        Salary = (user as Logistician)?.Salary ?? 0M,
                        WarehouseId = (user as Logistician)?.WarehouseId ?? 0
                    };
                case UserType.Courier:
                    goto case UserType.Logistician;
                case UserType.ClientBiz:
                    return new BusinessClientResponseDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                        UserType = user.UserType,
                        CompanyName = (user as BusinessClient)?.CompanyName,
                        Nip = (user as BusinessClient)?.Nip
                    };
                case UserType.ClientInd:
                    return new ClientResponseDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                        UserType = user.UserType,
                        Name = (user as Client)?.Name,
                        Surname = (user as Client)?.Surname,
                    };
                default:
                    return new UserResponseDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                        UserType = user.UserType,
                    };
            }
        }

        public static List<UserResponseDto> Of(IEnumerable<User> users)
        {
            return users.Select(Of).ToList();
        }
    }
}