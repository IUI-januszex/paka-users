using System.ComponentModel.DataAnnotations;

namespace PakaUsers.Dto.Requests
{
    public class AddAddressDto
    {
        [Required(ErrorMessage = "Address name is required")]
        public string AddressName { get; set; }
        [Required(ErrorMessage = "Personalities are required")]
        public string Personalities { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Building number is required")]
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
    }
}