using System.Threading.Tasks;
using BLL.Authentication.Interfaces;
using BLL.Authentication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PokeAPI.Controllers;
using Shared.Test.Helpers;

namespace Controller.Test
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        public AuthenticationControllerTests()
        {

        }

        [TestMethod]
        [ExpectedUserActionException(expectedExceptionMessage: null, expectedExceptionError: "Request body is invalid or empty")]
        public async Task RegisterWithNullShouldThrowException()
        {
            // Arrange
            var manager = new Mock<IAuthenticationManager>();
            var controller = new AuthenticationController(manager.Object);

            // Act
            var result = await controller.Register(null);
        }

        [TestMethod]
        [ExpectedUserActionException("ModelState is not valid", "Fake error that needs to be converted to an exception")]
        public async Task RegisterWithInvalidModelShouldThrowException()
        {
            // Arrange
            var manager = new Mock<IAuthenticationManager>();
            var controller = new AuthenticationController(manager.Object);
            controller.ModelState.AddModelError("UnitTest", "Fake error that needs to be converted to an exception");

            // Act
            var result = await controller.Register(new RegisterViewModel());
        }

        [TestMethod]
        public async Task RegisterWithValidModelShouldHitRegisterMethod()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                FirstName = "Brecht",
                LastName = "Carlier",
                Login = "ErazerBrecht",
                Password = "@Passw0rd"
            };

            var manager = new Mock<IAuthenticationManager>();
            manager.Setup(m => m.RegisterUser(model))
                .ReturnsAsync(new AuthenticationResultViewModel
                {
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    Login = model.Login,
                    Token = "UnitTestToken"
                });

            var controller = new AuthenticationController(manager.Object);


            // Act
            var result = await controller.Register(model);
            manager.Verify(m => m.RegisterUser(model), Times.Exactly(1));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));

            var jsonResult = (JsonResult) result;
            Assert.IsInstanceOfType(jsonResult.Value, typeof(AuthenticationResultViewModel));

            var jsonData = (AuthenticationResultViewModel) jsonResult.Value;
            Assert.AreEqual(model.Login, jsonData.Login);
            Assert.AreEqual(model.LastName, jsonData.LastName);
            Assert.AreEqual(model.FirstName, jsonData.FirstName);
            Assert.IsNotNull(jsonData.Token);
        }

        [TestMethod]
        [ExpectedUserActionException(expectedExceptionMessage: null, expectedExceptionError: "Request body is invalid or empty")]
        public async Task LoginWithNullShouldThrowException()
        {
            // Arrange
            var manager = new Mock<IAuthenticationManager>();
            var controller = new AuthenticationController(manager.Object);

            // Act
            var result = await controller.Login(null);
        }

        [TestMethod]
        [ExpectedUserActionException("ModelState is not valid", "Fake error that needs to be converted to an exception")]
        public async Task LoginWithInvalidModelShouldThrowException()
        {
            // Arrange
            var manager = new Mock<IAuthenticationManager>();
            var controller = new AuthenticationController(manager.Object);
            controller.ModelState.AddModelError("UnitTest", "Fake error that needs to be converted to an exception");

            // Act
            var result = await controller.Register(new RegisterViewModel());
        }

        [TestMethod]
        public async Task LoginWithValidModelShouldHitLoginMethod()
        {
            // Arrange
            const string login = "ErazerBrecht";
            const string firstName = "Brecht";
            const string lastName = "Carlier";
            const string token = "UnitTestToken";

            var model = new UserCredentialsViewModel
            {
                Login = login,
                Password = "@Passw0rd"
            };

            var manager = new Mock<IAuthenticationManager>();
            manager.Setup(m => m.LoginUser(model))
                .ReturnsAsync(new AuthenticationResultViewModel
                {
                    LastName = lastName,
                    FirstName = firstName,
                    Login = model.Login,
                    Token = token
                });

            var controller = new AuthenticationController(manager.Object);


            // Act
            var result = await controller.Login(model);
            manager.Verify(m => m.LoginUser(model), Times.Exactly(1));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));

            var jsonResult = (JsonResult)result;
            Assert.IsInstanceOfType(jsonResult.Value, typeof(AuthenticationResultViewModel));

            var jsonData = (AuthenticationResultViewModel)jsonResult.Value;
            Assert.AreEqual(login, jsonData.Login);
            Assert.AreEqual(lastName, jsonData.LastName);
            Assert.AreEqual(firstName, jsonData.FirstName);
            Assert.AreEqual(token, jsonData.Token);
        }
    }
}
