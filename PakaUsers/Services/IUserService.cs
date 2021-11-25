using PakaUsers.Model;

namespace PakaUsers.Services
{
    public interface IUserService
    {
        bool IsUserLogged();

        User GetCurrentUser();

        bool HasCurrentUserAnyRole(params UserType[] roles);
    }
}