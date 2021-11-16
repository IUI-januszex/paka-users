using System;
using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class AddressResponseDto 
    {
        public string PersonId { get; set; }
        public string AddressName { get; set; }
        public string Personalities { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }

        public static AddressResponseDto ToDto(string personId, AddressBookRecord addressBookRecord)
        {
            return new AddressResponseDto
            {
                PersonId = personId,
                AddressName = addressBookRecord.AddressName,
                Personalities = addressBookRecord.Personalities,
                Email = addressBookRecord.Email,
                City = addressBookRecord.City,
                Street = addressBookRecord.Street,
                Zip = addressBookRecord.Zip,
                BuildingNumber = addressBookRecord.BuildingNumber,
                ApartmentNumber = addressBookRecord?.ApartmentNumber
            };
        }
    }
}