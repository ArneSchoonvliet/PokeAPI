using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL_Database.Ef.Entities
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }

        public virtual ICollection<UserAnime> MyAnimes { get; set; }
    }
}
