using System;
using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class AddressResponseDto 
    {
        public string PersonId { get; set; }
        public string AddressName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }

        public static AddressResponseDto ToDto(string personId, Address address)
        {
            return new AddressResponseDto
            {
                PersonId = personId,
                AddressName = address.AddressName,
                Name = address?.Name,
                Surname = address?.Surname,
                Email = address.Email,
                City = address.City,
                Street = address.Street,
                Zip = address.Zip,
                BuildingNumber = address.BuildingNumber,
                ApartmentNumber = address?.ApartmentNumber,
                CompanyName = address?.CompanyName
            };
        }
    }
}