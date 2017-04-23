using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Anime.Interfaces;
using BLL.Anime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("anime")]
    [Produces("application/json")]
    public class AnimeController : Controller
    {
        private readonly IAnimeManager _animeManager;

        public AnimeController(IAnimeManager animeManager)
        {
            _animeManager = animeManager;
        }

        // GET anime/search/{{searchQuery}}
        /// <summary>
        /// Search for anime titles 
        /// </summary>
        /// <param name="query">Search query to find anime titles</param>
        /// <returns>List of animes containing the query in their title</returns>
        [HttpGet("search/{query}")]
        public async Task<List<PokemonListViewModel>> Search(string query)
        {
            return new List<PokemonListViewModel>();
        }
    }
}
