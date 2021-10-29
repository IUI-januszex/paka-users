using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PakaUsers.Controllers;
using PakaUsers.IdentityAuth;
using PakaUsers.Model;


namespace PakaUsers
{
    public class MyInitializer : IHostedService
    {
        private readonly UserManager<User> _adminManager;
        private readonly IUserRepository _userRepository;

        public MyInitializer(UserManager<User> adminManager, IUserRepository userRepository)
        {
            _adminManager = adminManager;
            _userRepository = userRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (AdminExists())
            {
                return Task.CompletedTask;
            }
            var admin = new Admin()
            {
                UserName = "admin",
                Email = "admin@januszex.pl"
            };
            return _adminManager.CreateAsync(admin, "admin");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // We have to implement this method too, because it is in the interface

            return Task.CompletedTask;
        }
        private bool AdminExists()
        {
            return _userRepository.QueryUsers().Any(user => user.UserType.Equals(UserType.Admin));
        }
    }
}