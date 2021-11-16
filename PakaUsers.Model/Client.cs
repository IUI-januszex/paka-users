using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class Client : User
    {
        public Client()
        {
            AddressBook = new List<AddressBookRecord>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<AddressBookRecord> AddressBook { get; set; }
        public override UserType UserType => UserType.ClientInd;
    }
}