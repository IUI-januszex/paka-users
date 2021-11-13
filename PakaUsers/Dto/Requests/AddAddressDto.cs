using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class AddAddressDto
    {
        public string AddressName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }
}