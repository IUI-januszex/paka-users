using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PakaUsers.Model;


namespace PakaUsers
{
    public class AdminInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public AdminInitializer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            if (!AdminExists(userRepository))
            {
                var admin = new Admin()
                {
                    UserName = "admin",
                    Email = "admin@januszex.pl"
                };
                var createdUser = userManager.CreateAsync(admin, "Admin123!").Result;
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private bool AdminExists(IUserRepository userRepository)
        {
            return userRepository.GetAll().Any(user => user.UserType.Equals(UserType.Admin));
        }
    }
}