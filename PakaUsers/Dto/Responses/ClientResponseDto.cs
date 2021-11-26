using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class ClientResponseDto : UserResponseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public static ClientResponseDto Of(Client user)
        {
            return new ClientResponseDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserType = user.UserType,
                Name = user.Name,
                Surname = user.Surname
            };
        }
    }
}