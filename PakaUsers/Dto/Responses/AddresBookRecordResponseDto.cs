using System;
using PakaUsers.Model;

namespace PakaUsers.Dto.Responses
{
    public class AddresBookRecordResponseDto 
    {
        public string PersonId { get; set; }
        public string AddressName { get; set; }
        public string Personalities { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }

        public static AddresBookRecordResponseDto ToDto(string personId, AddressBookRecord addressBookRecord)
        {
            return new AddresBookRecordResponseDto
            {
                PersonId = personId,
                AddressName = addressBookRecord.AddressName,
                Personalities = addressBookRecord.Personalities,
                Email = addressBookRecord.Email,
                City = addressBookRecord.City,
                Street = addressBookRecord.Street,
                PostalCode = addressBookRecord.PostalCode,
                BuildingNumber = addressBookRecord.BuildingNumber,
                FlatNumber = addressBookRecord?.FlatNumber
            };
        }
    }
}