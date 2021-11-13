using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class BusinessClient : User
    {
        public BusinessClient()
        {
            AddressBook = new List<Address>();
        }
        
        public string CompanyName { get; set; }
        public string Nip { get; set; }
        public List<Address> AddressBook { get; set; }
        public override UserType UserType => UserType.ClientBiz;
    }
}
