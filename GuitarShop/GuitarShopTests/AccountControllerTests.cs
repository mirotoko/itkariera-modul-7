using NUnit.Framework;
using GuitarShop.Controllers;
using GuitarShop.ViewModels;
using GuitarShop.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace GuitarShopTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<UserManager<User>> _userManager;
        private Mock<SignInManager<User>> _signInManager;
        private AccountController _controller;
        private Mock<RoleManager<IdentityRole>> _roleManager;

        [SetUp]
        public void Setup()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            _signInManager = new Mock<SignInManager<User>>(
                _userManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _controller = new AccountController(
                _signInManager.Object,
                _userManager.Object,
                null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManager = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object, null, null, null, null);

            _controller = new AccountController(
                _signInManager.Object,
                _userManager.Object,
                _roleManager.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            _controller?.Dispose();
        }

        // Login Tests
        [Test]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public async Task Login_ValidCredentials_RedirectsToHome()
        {
            var model = new LoginViewModel { Email = "test@example.com", Password = "Password123" };
            _signInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var result = await _controller.Login(model);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task Login_InvalidModel_ReturnsViewWithErrors()
        {
            _controller.ModelState.AddModelError("Email", "Required");
            var result = await _controller.Login(new LoginViewModel());
            Assert.IsInstanceOf<ViewResult>(result);
        }

        // Registration Tests
        [Test]
        public void Register_Get_ReturnsView()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public async Task Register_ValidUser_CreatesUserWithRole()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Pass123!"
            };

            _userManager.Setup(x => x.CreateAsync(It.Is<User>(u =>
                u.Email == model.Email &&
                u.UserName == model.Email &&
                u.FullName == model.Name),
                model.Password))
                .ReturnsAsync(IdentityResult.Success);

            _roleManager.Setup(x => x.RoleExistsAsync("User"))
                .ReturnsAsync(true);

            _userManager.Setup(x => x.AddToRoleAsync(It.Is<User>(u => u.Email == model.Email), "User"))
                .ReturnsAsync(IdentityResult.Success);

            _signInManager.Setup(x => x.SignInAsync(It.IsAny<User>(), false, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Login", ((RedirectToActionResult)result).ActionName);

            _userManager.Verify(x => x.CreateAsync(
                It.Is<User>(u =>
                    u.Email == model.Email &&
                    u.UserName == model.Email &&
                    u.FullName == model.Name),
                model.Password), Times.Once);

            _userManager.Verify(x => x.AddToRoleAsync(
                It.Is<User>(u => u.Email == model.Email),
                "User"), Times.Once);
        }

        [Test]
        public async Task Register_CreatesRoleWhenMissing()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Pass123!"
            };

            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);

            _roleManager.Setup(x => x.RoleExistsAsync("User"))
                .ReturnsAsync(false);

            _roleManager.Setup(x => x.CreateAsync(It.Is<IdentityRole>(r => r.Name == "User")))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(model);

            // Assert
            _roleManager.Verify(x => x.CreateAsync(
                It.Is<IdentityRole>(r => r.Name == "User")), Times.Once);
        }

        [Test]
        public async Task Register_InvalidModel_ReturnsViewWithErrors()
        {
            _controller.ModelState.AddModelError("Email", "Invalid email");
            var result = await _controller.Register(new RegisterViewModel());
            Assert.IsInstanceOf<ViewResult>(result);
        }

        // Password Change Tests
        [Test]
        public async Task ChangePassword_ValidRequest_UpdatesPassword()
        {
            var model = new ChangePasswordViewModel
            {
                Email = "user@test.com",
                NewPassword = "NewPass123!"
            };
            var user = new User { UserName = model.Email };

            _userManager.Setup(x => x.FindByNameAsync(model.Email)).ReturnsAsync(user);
            _userManager.Setup(x => x.RemovePasswordAsync(user)).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddPasswordAsync(user, model.NewPassword)).ReturnsAsync(IdentityResult.Success);

            var result = await _controller.ChangePassword(model);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Login", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task ChangePassword_UserNotFound_ReturnsViewWithError()
        {
            // Arrange
            var model = new ChangePasswordViewModel
            {
                Email = "nonexistent@test.com",
                NewPassword = "NewPass123!"
            };

            _userManager
                .Setup(x => x.FindByNameAsync(model.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.ChangePassword(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ModelState.IsValid, Is.False);
        }
        [Test]
        public void ChangePassword_Get_NullUsername_RedirectsToVerifyEmail()
        {
            // Act
            string a = null;
            var result = _controller.ChangePassword(a);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("VerifyEmail", redirectResult.ActionName);
            Assert.AreEqual("Account", redirectResult.ControllerName);
        }

        [Test]
        public void ChangePassword_Get_EmptyUsername_RedirectsToVerifyEmail()
        {
            // Act
            var result = _controller.ChangePassword("");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("VerifyEmail", redirectResult.ActionName);
        }

        [Test]
        public void ChangePassword_Get_ValidUsername_ReturnsViewWithModel()
        {
            // Arrange
            var testUsername = "user@example.com";

            // Act
            var result = _controller.ChangePassword(testUsername);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ChangePasswordViewModel>(viewResult.Model);
            Assert.AreEqual(testUsername, ((ChangePasswordViewModel)viewResult.Model).Email);
        }

        // Logout Test
        [Test]
        public async Task Logout_SignsOutAndRedirects()
        {
            _signInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);
            var result = await _controller.Logout();
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        // Email Verification Tests
        [Test]
        public void VerifyEmail_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.VerifyEmail();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public async Task VerifyEmail_ValidEmail_RedirectsToChangePassword()
        {
            var model = new VerifyEmailViewModel { Email = "test@example.com" };
            var user = new User { UserName = model.Email };
            _userManager.Setup(x => x.FindByNameAsync(model.Email)).ReturnsAsync(user);

            var result = await _controller.VerifyEmail(model);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("ChangePassword", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task VerifyEmail_InvalidEmail_ReturnsViewWithError()
        {
            var model = new VerifyEmailViewModel { Email = "bad@email.com" };
            _userManager.Setup(x => x.FindByNameAsync(model.Email)).ReturnsAsync((User)null);

            var result = await _controller.VerifyEmail(model);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsTrue(_controller.ModelState.ErrorCount > 0);
        }
    }
}