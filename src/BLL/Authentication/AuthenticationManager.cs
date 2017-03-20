using BLL.Authentication.Interfaces;
using BLL.Authentication.Options;
using BLL.Authentication.ViewModels;
using BLL.Exceptions;
using BLL.Extensions.Dictionary;
using BLL.Extensions.Identity;
using DAL.DbContext.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLL.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;

        public AuthenticationManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtIssuerOptions> jwtOptions,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AuthenticationManager>();
        }

        public async Task<AuthenticationResultViewModel> RegisterUser(RegisterViewModel credentials)
        {
            #region Argument Checks
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials), "Credentials cannot be null when registering as a new user");
            if (string.IsNullOrEmpty(credentials.Login))
                throw new ArgumentNullException(nameof(credentials.Login), "Username cannot be null or empty when registering as a new user");
            if (string.IsNullOrEmpty(credentials.FirstName))
                throw new ArgumentNullException(nameof(credentials.Login), $"{nameof(credentials.FirstName)} cannot be null or empty when registering as a new user");
            if (string.IsNullOrEmpty(credentials.LastName))
                throw new ArgumentNullException(nameof(credentials.Login), $"{nameof(credentials.LastName)} cannot be null or empty when registering as a new user");
            if (string.IsNullOrEmpty(credentials.Password))
                throw new ArgumentNullException(nameof(credentials.Login), "Password cannot be null or empty when registering as a new user");
            #endregion

            _logger.LogDebug($"Trying to create a new user:{Environment.NewLine}Login: {credentials.Login}{Environment.NewLine}Firstname: {credentials.FirstName}{Environment.NewLine}Lastname: {credentials.LastName} ");

            // TODO Automapper
            var user = new User
            {
                UserName = credentials.Login,
                Created = DateTime.UtcNow,
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
            };

            var result = await _userManager.CreateAsync(user, credentials.Password);
            if (!result.Succeeded)
            {
                throw new UserActionException(result.ToHashtable(), "User Creation Failed");
            }

            _logger.LogDebug($"New user created:{Environment.NewLine}Login: {user.UserName}{Environment.NewLine}Firstname: {user.FirstName}{Environment.NewLine}Lastname: {user.LastName} ");
            return await LoginUser(credentials);
        }

        public async Task<AuthenticationResultViewModel> LoginUser(UserCredentialsViewModel credentials)
        {
            #region Argument Checks
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials), "Credentials cannot be null when generating an access token");
            if (string.IsNullOrEmpty(credentials.Login))
                throw new ArgumentNullException(nameof(credentials.Login), "Username cannot be null or empty to generate an access token");
            if (string.IsNullOrEmpty(credentials.Password))
                throw new ArgumentNullException(nameof(credentials.Login), "Password cannot be null or empty to generate an access token");
            #endregion

            _logger.LogDebug($"Trying to sign in a user:{Environment.NewLine}Login: {credentials.Login}");
            var user = await _userManager.FindByNameAsync(credentials.Login);

            if (user == null)
                throw new InvalidCredentialException($"User with {nameof(credentials.Login).ToLower()}: {credentials.Login} cannot be found!");

            var identity = await GetIdentity(user, credentials.Password);
            var encodedJwt = await identity.GenerateJwtToken(_jwtOptions);

            // Update last login data
            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            _logger.LogDebug($"User is succesfully authenticated:{Environment.NewLine}Login: {user.UserName}{Environment.NewLine}Firstname: {user.FirstName}{Environment.NewLine}Lastname: {user.LastName} ");
            return new AuthenticationResultViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.UserName,
                Token = encodedJwt
            };
        }

        #region Helpers
        private async Task<ClaimsIdentity> GetIdentity(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException($"{nameof(user)} is 'null' when trying to retrieve it's identity");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (result.RequiresTwoFactor)
                throw new NotImplementedException("Two Factor Authentication is not implemented while signing in!");
            
            if (!result.Succeeded)
                throw new AuthenticationException($"User {user.UserName} / {user.Id} is not allowed to sign in with the provided credentials");

            
            var claims = await _userManager.GetClaimsAsync(user);
            return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), claims);
        }
        #endregion
    }
}
