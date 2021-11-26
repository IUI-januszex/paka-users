using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class BusinessClientResponseDto : UserResponseDto
    {
        public string CompanyName { get; set; }
        public string Nip { get; set; }

        public static BusinessClientResponseDto Of(BusinessClient user)
        {
            return new BusinessClientResponseDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                UserType = user.UserType,
                CompanyName = user.CompanyName,
                Nip = user.Nip
            };
        }
    }
}