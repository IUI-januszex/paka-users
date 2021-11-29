using System.Collections.Generic;
using System.Linq;

namespace PakaUsers.Model
{
    public interface IUserRepository
    {
        User Get(string id);
        IEnumerable<User> GetAll();
        void Insert(User user);
        void Delete(string id);
        void Update(User user);
        void Save();
        public void Anonymize(string id);
    }
}