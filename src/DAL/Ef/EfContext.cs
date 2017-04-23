using DAL_Database.Ef.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL_Database.Ef
{
    public class EfContext: IdentityDbContext<User>
    {
        public EfContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<UserAnime> UserAnimes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAnime>()
                .HasIndex(ua => ua.AnimeId);

            base.OnModelCreating(builder);
        }
    }

    public class EfContextFactory : IDbContextFactory<EfContext>
    {
        public EfContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<EfContext>();
            builder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = AnimeApi; Trusted_Connection = True;");
            return new EfContext(builder.Options);
        }
    }
}
