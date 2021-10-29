﻿using System.Collections.Generic;
using System.Linq;

namespace PakaUsers.Model
{
    public interface IUserRepository
    {
        User Get(string id);
        IEnumerable<User> GetAll();
        void Insert(User user);
        void Delete(User user);
        void Update(User user);
        void Save();
        public IQueryable<User> QueryUsers();
    }
}