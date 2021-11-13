namespace PakaUsers.Model
{
    public class Address
    {
        public long Id { get; set; }
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
        
        public string BusinessClientId { get; set; }
        public string ClientId { get; set; }
    }
}