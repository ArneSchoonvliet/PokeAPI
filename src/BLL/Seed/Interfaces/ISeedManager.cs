using System.Threading.Tasks;

namespace BLL.Seed.Interfaces
{
    public interface ISeedManager
    {
        void Seed();
        Task SeedPokemons();
    }
}