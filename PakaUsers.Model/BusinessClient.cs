using System;

namespace PakaUsers.Model
{
    public class BusinessClient : User
    {
        public string CompanyName { get; set; }
        public string Nip { get; set; }
        public override UserType UserType => UserType.ClientBiz;
    }
}
