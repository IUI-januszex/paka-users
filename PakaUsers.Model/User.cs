using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace PakaUsers.Model
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
    }
}