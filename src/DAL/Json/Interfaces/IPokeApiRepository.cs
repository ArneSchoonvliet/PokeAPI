using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Json.Entities;

namespace DAL.Json.Interfaces
{
    public interface IPokeApiRepository
    {
        Task<IList<PokemonJson>> GetPokemons(int start, int end);
    }
}