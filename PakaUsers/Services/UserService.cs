using System.Linq;
using Microsoft.AspNetCore.Http;
using PakaUsers.Model;

namespace PakaUsers.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserLogged()
        {
            var user = GetCurrentUser();
            return user is {IsActive: true};
        }

        public User GetCurrentUser()
        {
            var id = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (id == null)
                return null;
            var user = _userRepository.Get(id);
            return  user.IsActive ? user : null;
        }

        public bool HasCurrentUserAnyRole(params UserType[] roles)
        {
            var user = GetCurrentUser();
            return user != null && roles.Any(userType => user.UserType == userType);
        }
    }
}