namespace PakaUsers.Model
{
    public class AddressBookRecord
    {
        public long Id { get; set; }
        public string AddressName { get; set; }
        public string Personalities {get;set;}
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        
        public string BusinessClientId { get; set; }
        public string ClientId { get; set; }
    }
}