using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class Client : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public override UserType UserType => UserType.ClientInd;
    }
}