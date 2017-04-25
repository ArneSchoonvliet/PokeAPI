using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Anime.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("anime")]
    [Produces("application/json")]
    public class AnimeController : Controller
    {
        private readonly IAnimeManager _animeManager;

        public AnimeController(IAnimeManager animeManager)
        {
            _animeManager = animeManager;
        }

        // GET anime/{{id}}
        /// <summary>
        /// Get every detail of a specific anime serie by id 
        /// </summary>
        /// <param name="id">Id of anime</param>
        /// <returns>AnimeViewModel</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _animeManager.GetById(id));
        }

        // GET anime/search/{{searchQuery}}
        /// <summary>
        /// Search for anime titles 
        /// </summary>
        /// <param name="query">Search query to find anime titles</param>
        /// <param name="page">Index used for pagination</param>
        /// <returns>List of animes containing the query in their title</returns>
        [HttpGet("search/{query}")]
        public async Task<IActionResult> Search(string query, int page = 0)
        {
            return Ok(await _animeManager.SearchByKeyword(query, page));
        }
    }
}
