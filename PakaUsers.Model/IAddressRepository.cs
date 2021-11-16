using System.Collections.Generic;

namespace PakaUsers.Model
{
    public interface IAddressRepository
    {
        public List<AddressBookRecord> GetByUserId(string userId);
    }
}
