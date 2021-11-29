using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PakaUsers.Model
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public User Get(string id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Insert(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(string id)
        {
            _context.Users.Remove(Get(id));
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Anonymize(string id)
        {
            var user = Get(id);
            Update(user);

            user.UserName = null;
            user.NormalizedUserName = null;
            user.Email = null;
            user.NormalizedEmail = null;
            user.EmailConfirmed = false;
            user.PasswordHash = null;
            user.SecurityStamp = null;
            user.ConcurrencyStamp = null;
            user.PhoneNumber = null;
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;
            user.IsActive = false;

            switch (user)
            {
                case Client c:
                    c.Name = null;
                    c.Surname = null;
                    c.AddressBook = null;
                    break;
                case BusinessClient b:
                    b.Nip = null;
                    b.AddressBook = null;
                    b.CompanyName = null;
                    break;
                case Worker w:
                    w.Name = null;
                    w.Salary = 0;
                    w.Surname = null;
                    w.WarehouseId = -1;
                    w.WarehouseType = null;
                    break;
            }
            Save();
        }
    }
}