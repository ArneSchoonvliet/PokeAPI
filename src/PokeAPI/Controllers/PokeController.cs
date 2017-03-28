using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Pokemon.Interfaces;
using BLL.Pokemon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PokeController : Controller
    {
        private readonly IPokemonManager _pokeManager;

        public PokeController(IPokemonManager pokeManager)
        {
            _pokeManager = pokeManager;
        }
        // GET api/values
        [HttpGet]
        public async Task<List<PokemonListViewModel>> Get()
        {
            return await _pokeManager.GetAllPokemons();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Mew";
        }
    }
}
