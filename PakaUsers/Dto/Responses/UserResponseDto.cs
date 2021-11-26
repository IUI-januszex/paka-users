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
                    return WorkerResponseDto.Of((Worker)user);
                case UserType.Courier:
                    goto case UserType.Logistician;
                case UserType.ClientBiz:
                    return BusinessClientResponseDto.Of((BusinessClient) user);
                case UserType.ClientInd:
                    return ClientResponseDto.Of((Client) user);
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

        public static IEnumerable<UserResponseDto> Of(IEnumerable<User> users)
        {
            return users.Select(Of);
        }
        /*public static List<UserResponseDto> OfLogisticians(IEnumerable<User> users)
        {
            var logs = users.Select(user => user as Logistician).ToList();
            var t = logs.Select(Of).ToList();
            var x = 0;
            return logs.Select(Of).ToList();
        }*/
    }
}