using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DAL.DbContext.Entities
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
