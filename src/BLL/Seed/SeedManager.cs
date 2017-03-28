using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Seed.Interfaces;
using DAL.DbContext.Entities;
using DAL.DbContext.Interfaces;
using DAL.Json;
using DAL.Json.Entities;
using DAL.Json.Interfaces;

namespace BLL.Seed
{
    public class SeedManager : ISeedManager
    {
        private readonly IPokeApiRepository _pokeApiRepository;
        private readonly IPokemonRepository _pokeDbRepository;

        private readonly IMapper _mapper;

        public SeedManager(IPokeApiRepository pokeApiRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokeApiRepository = pokeApiRepository;
            _pokeDbRepository = pokemonRepository;
            _mapper = mapper;
        }

        public void Seed()
        {
            // Seeding a database only happens once.
            // No need for asynchronous code
            
            SeedPokemons().GetAwaiter().GetResult();
        }

        public async Task SeedPokemons()
        {
            if (!await _pokeDbRepository.Any())
            {
                var jsonResults = await RequestPokemonsFromApi();
                var entities = _mapper.Map<List<Pokemon>>(jsonResults);
                _pokeDbRepository.AddRange(entities);
                await _pokeDbRepository.Save();
            }
        }

        private async Task<List<PokemonJson>> RequestPokemonsFromApi()
        {
            return (await _pokeApiRepository.GetPokemons(1, 151)).ToList();
        }
    }
}