using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using BLL.Authentication.Options;
using BLL.Authentication.ViewModels;
using DAL_Database.Ef;
using DAL_Database.Ef.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Test.Helpers;

namespace BLL.Test
{
    [TestClass]
    public class AuthenticationManagerUnitTests
    {
        private const string Audience = "TestAudience";
        private const string Issuer = "TestIssuer";
        private const string TestPassword = "@Passw0rd";
        private readonly User _testUser;
        private readonly IAuthenticationManager _manager;
        private readonly UserManager<User> _userManager;

        public AuthenticationManagerUnitTests()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddEntityFramework()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<EfContext>(options => options.UseInMemoryDatabase());

            services.AddIdentity<User, IdentityRole>()
                 .AddEntityFrameworkStores<EfContext>();

            var serviceProvider = services.BuildServiceProvider();

            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var signinManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            _testUser = new User {UserName = "ErazerBrecht", FirstName = "Erazer", LastName = "Brecht"};
            _userManager.CreateAsync(_testUser).Wait();
            _userManager.AddPasswordAsync(_testUser, TestPassword).Wait();

            _manager = new AuthenticationManager(_userManager, signinManager, StubJwtOptions(), new LoggerFactory());
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
        [ExpectedUserActionException("User Creation Failed", "Passwords must have at least one")]
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
        [ExpectedUserActionException("User Creation Failed", "User name", "is already taken.")]
        public async Task RegisterWithAlreadyExistingLoginShouldThrowException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = _testUser.UserName,
                FirstName = _testUser.FirstName,
                LastName = _testUser.LastName,
                Password = TestPassword
            };

            // Act
            await _manager.RegisterUser(credentials);
        }

        [TestMethod]
        public async Task RegisterValidUserShouldSucceed()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Login = "ErazerBrecht2",
                FirstName = "Erazer",
                LastName = "Brecht",
                Password = "@Passw0rd"
            };

            // Act
            await _manager.RegisterUser(credentials);
            var createdUser = await _userManager.FindByNameAsync(credentials.Login);
            var password = await _userManager.CheckPasswordAsync(createdUser, credentials.Password);

            // Assert
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(credentials.FirstName, createdUser.FirstName); 
            Assert.AreEqual(credentials.LastName, createdUser.LastName);
            Assert.AreEqual(DateTime.UtcNow.Date, createdUser.Created.Date);
            Assert.AreEqual(DateTime.UtcNow.Hour, createdUser.Created.Hour);
            Assert.AreEqual(DateTime.UtcNow.Minute, createdUser.Created.Minute);
            Assert.IsTrue(password);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task LoginWithoutCredentialsShouldThrowException()
        {
            await _manager.LoginUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task LoginWithoutLoginShouldThrowException()
        {
            // Arrange
            var credentials = new UserCredentialsViewModel()
            {
                Login = null,
                Password = "@Passw0rd"
            };

            // Act
            await _manager.LoginUser(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task LoginWithoutPasswordShouldThrowException()
        {
            // Arrange
            var credentials = new UserCredentialsViewModel
            {
                Login = "ErazerBrecht",
                Password = null
            };

            // Act
            await _manager.LoginUser(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialException))]
        public async Task LoginWithNonExistingUserShouldThrowException()
        {
            // Arrange
            var user = new UserCredentialsViewModel()
            {
                Login = "ErazerBrecht3",
                Password = "@Passw0rd"
            };

            // Act
           await  _manager.LoginUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public async Task LoginWithWrongPasswordShouldThrowException()
        {
            // Arrange
            var user = new UserCredentialsViewModel
            {
                Login = _testUser.UserName,
                Password = TestPassword + "UNVALID"
            };

            // Act
            await _manager.LoginUser(user);
        }

        [TestMethod]
        public async Task LoginWithCorrectCredentialsShouldReturnToken()
        {
            // Arrange
            var user = new UserCredentialsViewModel()
            {
                Login = _testUser.UserName,
                Password = TestPassword
            };

            // Act
            var result = await _manager.LoginUser(user);
            var savedUser = await _userManager.FindByNameAsync(user.Login);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AuthenticationResultViewModel));
            Assert.AreEqual(_testUser.UserName, result.Login);
            Assert.AreEqual(_testUser.FirstName, result.FirstName);
            Assert.AreEqual(_testUser.LastName, result.LastName);
            Assert.IsNotNull(savedUser);
            Assert.AreEqual(DateTime.UtcNow.Date, savedUser.LastLogin.Date);
            Assert.AreEqual(DateTime.UtcNow.Hour, savedUser.LastLogin.Hour);
            Assert.AreEqual(DateTime.UtcNow.Minute, savedUser.LastLogin.Minute);
            Assert.IsNotNull(result.Token);
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
