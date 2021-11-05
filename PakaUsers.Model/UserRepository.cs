using System;
using System.Collections.Generic;
using System.Linq;

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

        public IQueryable<User> QueryUsers()
        {
            return _context.Users;
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
    }
}