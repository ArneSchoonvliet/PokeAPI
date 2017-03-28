using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Pokemon.Interfaces;
using BLL.Pokemon.ViewModels;
using DAL.DbContext.Interfaces;

namespace BLL.Pokemon
{
    public class PokemonManager : IPokemonManager
    {
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonManager(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
        }

        public async Task<List<PokemonListViewModel>> GetAllPokemons()
        {
            return _mapper.Map<List<PokemonListViewModel>>(await _pokemonRepository.GetAllPokemons());
        }

        //public Task GetMyPokemons()
        //{
        //    
        //}

        //public Task GetMyPokemon(int pokedexId)
        //{
        //   
        //}

    }
}
