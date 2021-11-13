using System.Collections.Generic;

namespace PakaUsers.Model
{
    public interface IAddressRepository
    {
        public List<Address> GetByUserId(string userId);
    }
}
