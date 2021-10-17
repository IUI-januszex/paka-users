using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PakaUsers.Model
{
    public class UserContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID =postgres;Password=postgres;" +
                                     "Server=localhost;Port=5432;Database=users_database;" +
                                     "Integrated Security=true;Pooling=true;");
        }
    }
}