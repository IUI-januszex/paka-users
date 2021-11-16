using System.Collections.Generic;
using System.Linq;

namespace PakaUsers.Model
{
    public class AddressRepository : IAddressRepository
    {      
        private readonly UserContext _context;

        public AddressRepository(UserContext context)
        {
            _context = context;
        }
        
        public List<AddressBookRecord> GetByUserId(string userId)
        {
            return _context.Address.Where(address => address.BusinessClientId == userId || address.ClientId == userId)
                .ToList();
        }
    }
}