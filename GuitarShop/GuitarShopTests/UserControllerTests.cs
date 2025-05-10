using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GuitarShop.Controllers;
using GuitarShop.Data;
using GuitarShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace GuitarShopTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private GuitarShopContext _context;
        private UserController _controller;
        private User _testUser;
        private List<Guitar> _testGuitars;
        private List<Purchase> _testPurchases;

        [SetUp]
        public void Setup()
        {
            // Setup mock UserManager
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            // Setup in-memory database instead of mocking DbContext
            var options = new DbContextOptionsBuilder<GuitarShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GuitarShopContext(options);

            // Setup test data
            _testUser = new User { Id = "test-user-id", UserName = "testuser", Cart = new List<string> { "Guitar1", "Guitar2" } };
            _testGuitars = new List<Guitar>
            {
                new Guitar { Name = "Guitar1", Price = 1000 },
                new Guitar { Name = "Guitar2", Price = 2000 },
                new Guitar { Name = "Guitar3", Price = 3000 }
            };
            _testPurchases = new List<Purchase>
            {
                new Purchase("OldGuitar", "other-user-id")
            };

            // Add test data to in-memory database
            _context.Guitar.AddRange(_testGuitars);
            _context.Purchase.AddRange(_testPurchases);
            _context.SaveChanges();

            // Setup controller with dependencies
            _controller = new UserController(_context, _mockUserManager.Object);

            // Mock User identity
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
            _controller?.Dispose();
        }

        [Test]
        public async Task Cart_ReturnsViewWithUserGuitars()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _controller.Cart();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Guitar>;

            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.Any(g => g.Name == "Guitar1"));
            Assert.IsTrue(model.Any(g => g.Name == "Guitar2"));
        }

        [Test]
        public async Task Cart_ReturnsEmptyList_WhenCartIsEmpty()
        {
            // Arrange
            _testUser.Cart = new List<string>(); 
            _mockUserManager
                .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _controller.Cart() as ViewResult;

            // Assert
            var model = result?.Model as List<Guitar>;
            Assert.NotNull(model);
            Assert.IsEmpty(model);
        }


        [Test]
        public async Task Purchase_CreatesPurchasesAndClearsCart()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            var initialPurchaseCount = _context.Purchase.Count();

            // Act
            var result = await _controller.Purchase();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);

            // Verify purchases were added to context
            var finalPurchaseCount = _context.Purchase.Count();
            Assert.AreEqual(initialPurchaseCount + 2, finalPurchaseCount);

            // Verify cart was cleared
            _mockUserManager.Verify(um => um.UpdateAsync(It.Is<User>(u => u.Cart.Count == 0)), Times.Once);
        }

        [Test]
        public async Task Purchase_HandlesEmptyCart()
        {
            // Arrange
            _testUser.Cart.Clear();
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);

            var initialPurchaseCount = _context.Purchase.Count();

            // Act
            var result = await _controller.Purchase();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);

            // Verify no purchases were added
            var finalPurchaseCount = _context.Purchase.Count();
            Assert.AreEqual(initialPurchaseCount, finalPurchaseCount);
        }

        [Test]
        public async Task EmptyCart_ClearsUserCartAndRedirects()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.EmptyCart();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;

            Assert.AreEqual("Cart", redirectResult.ActionName);

            // Verify cart was cleared
            _mockUserManager.Verify(um => um.UpdateAsync(It.Is<User>(u => u.Cart.Count == 0)), Times.Once);
        }

        [Test]
        public async Task EmptyCart_HandlesAlreadyEmptyCart()
        {
            // Arrange
            _testUser.Cart.Clear();
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _controller.EmptyCart();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            // Verify update was still called (even though cart was already empty)
            _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
        }
    }
}