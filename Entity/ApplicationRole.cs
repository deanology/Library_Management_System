using System;
using Microsoft.AspNetCore.Identity;

namespace Library_Management_System.Entity
{
    public class ApplicationRole : IdentityRole
    {
        public string RoleDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
