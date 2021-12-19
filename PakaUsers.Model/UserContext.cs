using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PakaUsers.Model
{
    public class UserContext : IdentityDbContext<User>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<BusinessClient> BusinessClient { get; set; }
        public DbSet<Logistician> Logistician { get; set; }
        public DbSet<Courier> Courier { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<AddressBookRecord> AddressBook { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID =postgres;Password=postgres;" +
                                     "Server=postgres;Port=5432;Database=users_database;" +
                                     "Integrated Security=true;Pooling=true;");
        }
        
    }
}