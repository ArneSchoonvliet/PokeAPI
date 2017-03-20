using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokeAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PokeController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Bulbasaur", "Squirtle", "Charmender" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Mew";
        }
    }
}
