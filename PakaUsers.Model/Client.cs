using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class Client : User
    {
        public Client()
        {
            AddressBook = new List<Address>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Address> AddressBook { get; set; }
        public override UserType UserType => UserType.ClientInd;
    }
}