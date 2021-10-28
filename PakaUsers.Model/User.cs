using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace PakaUsers.Model
{
    public abstract class User : IdentityUser
    {
        public abstract UserType UserType { get; }
        public bool IsActive { get; set; }
    }
}