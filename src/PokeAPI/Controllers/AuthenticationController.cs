using BLL.Authentication.Interfaces;
using BLL.Authentication.ViewModels;
using BLL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PokeAPI.Helpers;
using System.Threading.Tasks;

namespace PokeAPI.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationManager _manager;

        public AuthenticationController(IAuthenticationManager manager)
        {
            _manager = manager;
        }

        // POST: api/authentication/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsViewModel model)
        {
            if (model == null)
                throw new UserActionException("Body", "Request body is invalid or empty", $"Model (Request body) is null when executing {nameof(Login)} method inside {nameof(AuthenticationController)}");

            if (!ModelState.IsValid)
                throw new UserActionException(ModelState.ToHashtable(), $"ModelState is not valid when executing {nameof(Login)} method inside {nameof(AuthenticationController)}");

            return Json(await _manager.LoginUser(model));
        }

        // POST: api/authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if(model == null)
                throw new UserActionException("Body", "Request body is invalid or empty", $"Model (Request body) is null when executing {nameof(Register)} method inside {nameof(AuthenticationController)}");

            if (!ModelState.IsValid)
                throw new UserActionException(ModelState.ToHashtable(), $"ModelState is not valid when executing {nameof(Register)} method inside {nameof(AuthenticationController)}");

            return Json(await _manager.RegisterUser(model));
        }
    }
}
