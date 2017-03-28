using DAL.DbContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.DbContext
{
    public class PokeContext: IdentityDbContext<User>
    {
        public PokeContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Pokemon>()
                .HasIndex(e => e.PokedexId)
                .IsUnique()
                .ForSqlServerIsClustered();
        }
    }

    public class PokeContextFactory : IDbContextFactory<PokeContext>
    {
        public PokeContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<PokeContext>();
            builder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = PokeApi; Trusted_Connection = True;");
            return new PokeContext(builder.Options);
        }
    }
}
