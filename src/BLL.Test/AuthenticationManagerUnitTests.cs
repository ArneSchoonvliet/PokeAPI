using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using BLL.Authentication.Options;
using BLL.Authentication.ViewModels;
using DAL.DbContext.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            _manager = new AuthenticationManager(MockUserManager<User>(), MockSigninManager<User>(), StubJwtOptions(), new LoggerFactory());
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


        #region Helpers
        private static Mock<IUserStore<TUser>> MockUserStore<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return store;
        }

        private static UserManager<TUser> MockUserManager<TUser>() where TUser : class
        {
            IList<IUserValidator<TUser>> userValidators = new List<IUserValidator<TUser>>();
            IList<IPasswordValidator<TUser>> passwordValidators = new List<IPasswordValidator<TUser>>();

            var store = MockUserStore<TUser>();
            userValidators.Add(new UserValidator<TUser>());
            passwordValidators.Add(new PasswordValidator<TUser>());
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, userValidators, passwordValidators, null, null, null, null);
            return mgr.Object;
        }

        private static SignInManager<TUser> MockSigninManager<TUser>() where TUser : class
        {
            var context = new Mock<HttpContext>();
            var mock = 
                new Mock<SignInManager<TUser>>(MockUserManager<TUser>(),
                    new HttpContextAccessor { HttpContext = context.Object },
                    new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                    null, null)
                {
                    CallBase = true
                };

            return mock.Object;
        }

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
