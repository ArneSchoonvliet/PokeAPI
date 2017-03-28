using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Pokemon.ViewModels;

namespace BLL.Pokemon.Interfaces
{
    public interface IPokemonManager
    {
        Task<List<PokemonListViewModel>> GetAllPokemons();
    }
}