using System;
using System.Threading.Tasks;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using BLL.Authentication.Options;
using BLL.Authentication.ViewModels;
using BLL.Test.Helpers;
using DAL.DbContext;
using DAL.DbContext.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Test
{
    [TestClass]
    public class AuthenticationManagerUnitTests
    {
        private const string Audience = "TestAudience";
        private const string Issuer = "TestIssuer";
        private readonly IAuthenticationManager _manager;

        public AuthenticationManagerUnitTests()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddEntityFramework()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<PokeContext>(options => options.UseInMemoryDatabase());

            services.AddIdentity<User, IdentityRole>()
                 .AddEntityFrameworkStores<PokeContext>(); ;

            var serviceProvider = services.BuildServiceProvider();

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var signinManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            userManager.CreateAsync(new User { UserName = "ErazerBrecht", FirstName = "Erazer", LastName = "Brecht" }).Wait();

            _manager = new AuthenticationManager(userManager, signinManager, StubJwtOptions(), new LoggerFactory());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterWithoutCredentialsShouldThrowException()
        {
            await _manager.RegisterUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterWithoutLoginShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = null,
                FirstName = "Erazer",
                LastName = "Brecht",
                Password = "@Passw0rd"
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterWithoutFirstNameShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht",
                FirstName = null,
                LastName = "Brecht",
                Password = "@Passw0rd"
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterWithoutLastNameShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht",
                FirstName = "Erazer",
                LastName = null,
                Password = "@Passw0rd"
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterWithoutPasswordShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht",
                FirstName = "Erazer",
                LastName = "Brecht",
                Password = null
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        [ExpectedUserActionException("User Creation Failed", "PasswordRequires")]
        public async Task RegisterWithInvalidPasswordShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht",
                FirstName = "Erazer",
                LastName = "Brecht",
                Password = "Password"
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        [ExpectedUserActionException("User Creation Failed", "DuplicateUserName")]
        public async Task RegisterWithAlreadyExistingLoginShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht",
                FirstName = "Erazer",
                LastName = "Brecht",
                Password = "@Passw0rd"
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        #region Helpers
        private static IOptions<JwtIssuerOptions> StubJwtOptions()
        {
            return Options.Create(new JwtIssuerOptions
            {
                Audience = Audience,
                Issuer = Issuer
            });
        }
        #endregion
    }
}
