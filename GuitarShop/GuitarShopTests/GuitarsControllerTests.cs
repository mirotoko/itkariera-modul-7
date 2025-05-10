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
    public class GuitarsControllerTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private GuitarShopContext _context;
        private GuitarsController _controller;
        private User _testUser;

        [SetUp]
        public void Setup()
        {
            // Setup mock UserManager
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            // Setup in-memory database
            var options = new DbContextOptionsBuilder<GuitarShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GuitarShopContext(options);

            // Seed test data
            _context.Guitar.AddRange(new List<Guitar>
            {
                new Guitar { Name = "C40", Brand = "Yamaha", Type = "Classical", Body = "4/4", Price = 119, Availability = 25 },
                new Guitar { Name = "Les Paul", Brand = "Gibson", Type = "Electric", Body = "Single Cut", Price = 2499, Availability = 8 },
                new Guitar { Name = "C10", Brand = "Cordoba", Type = "Classical", Body = "4/4", Price = 1249, Availability = 8 }
            });

            _context.SaveChanges();

            // Setup test user
            _testUser = new User { Id = "test-user", UserName = "testuser", Cart = new List<string>() };
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(_testUser);

            // Setup controller
            _controller = new GuitarsController(_context, _mockUserManager.Object);

            // Mock HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "test-user"),
                        new Claim(ClaimTypes.Name, "testuser")
                    }, "test"))
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAllGuitars_WhenNoSearchString()
        {
            // Act
            var result = await _controller.Index(null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Guitar>;
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public async Task Index_ReturnsFilteredGuitars_WhenSearchStringMatches()
        {
            // Act - Search by brand
            var result = await _controller.Index("Yamaha");

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Guitar>;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("C40", model[0].Name);

            // Act - Search by body type
            result = await _controller.Index("Cordoba");
            viewResult = result as ViewResult;
            model = viewResult.Model as List<Guitar>;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("C10", model[0].Name);
        }

        [Test]
        public async Task Index_ReturnsEmptyList_WhenNoMatchesFound()
        {
            // Act
            var result = await _controller.Index("Nonexistent");

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Guitar>;
            Assert.IsEmpty(model);
        }

        [Test]
        public async Task Details_ReturnsGuitar_WhenNameExists()
        {
            // Act
            var result = await _controller.Details("Les Paul");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Guitar;
            Assert.AreEqual("Les Paul", model.Name);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenNameDoesNotExist()
        {
            // Act
            var result = await _controller.Details("Nonexistent");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenNameIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_RedirectsToIndex_WhenModelIsValid()
        {
            // Arrange
            var newGuitar = new Guitar
            {
                Name = "KVXMG King V Satin BK",
                Brand = "Jackson",
                Type = "Electric",
                Body = "Heavy",
                Price = 1200,
                Availability = 4
            };

            // Act
            var result = await _controller.Create(newGuitar);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual(4, _context.Guitar.Count());
            Assert.IsTrue(_context.Guitar.Any(g => g.Name == "KVXMG King V Satin BK"));
        }

        [Test]
        public async Task Create_ReturnsView_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Test error");
            var invalidGuitar = new Guitar();

            // Act
            var result = await _controller.Create(invalidGuitar);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(invalidGuitar, viewResult.Model);
        }

        [Test]
        public async Task Edit_ReturnsGuitar_WhenNameExists()
        {
            // Act
            var result = await _controller.Edit("C40");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Guitar;
            Assert.AreEqual("C40", model.Name);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenNameDoesNotExist()
        {
            // Act
            var result = await _controller.Edit("Nonexistent");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenNameIsNull()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_RedirectsToIndex_WhenUpdateIsSuccessful()
        {
            // Arrange
            var guitar = await _context.Guitar.FirstAsync(g => g.Name == "Les Paul");
            guitar.Price = 2500;

            // Act
            var result = await _controller.Edit("Les Paul", guitar);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var updatedGuitar = await _context.Guitar.FindAsync("Les Paul");
            Assert.AreEqual(2500, updatedGuitar.Price);
        }

        [Test]
        public async Task Delete_ReturnsGuitar_WhenNameExists()
        {
            // Act
            var result = await _controller.Delete("C10");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Guitar;
            Assert.AreEqual("C10", model.Name);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenNameDoesNotExist()
        {
            // Act
            var result = await _controller.Delete("Nonexistent");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenNameIsNull()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesGuitarAndRedirects()
        {
            // Arrange
            var initialCount = _context.Guitar.Count();

            // Act
            var result = await _controller.DeleteConfirmed("C40");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual(initialCount - 1, _context.Guitar.Count());
            Assert.IsNull(await _context.Guitar.FindAsync("C40"));
        }

        [Test]
        public async Task AddToCart_AddsItemsToUserCart()
        {
            // Arrange
            var data = new GuitarsController.AddToCartData
            {
                GuitarName = "Les Paul",
                Count = 2
            };

            // Act
            var result = await _controller.AddToCart(data);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(2, _testUser.Cart.Count);
            Assert.AreEqual("Les Paul", _testUser.Cart[0]);
            Assert.AreEqual("Les Paul", _testUser.Cart[1]);
            _mockUserManager.Verify(um => um.UpdateAsync(_testUser), Times.Once);
        }

        [Test]
        public async Task AddToCart_ReturnsOkWithCorrectMessage()
        {
            // Arrange
            var data = new GuitarsController.AddToCartData
            {
                GuitarName = "C10",
                Count = 1
            };

            // Act
            var result = await _controller.AddToCart(data) as OkObjectResult;

            // Assert
            Assert.AreEqual("C10 1", result.Value);
        }
    }
}