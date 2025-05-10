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
    public class PurchasesControllerTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private GuitarShopContext _context;
        private PurchasesController _controller;

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
                new Guitar { Name = "C40", Availability = 5 },
                new Guitar { Name = "Les Paul", Availability = 3 }
            });

            _context.Purchase.AddRange(new List<Purchase>
            {
                new Purchase { GuitarName = "C40", IsProcessed = false },
                new Purchase { GuitarName = "Les Paul", IsProcessed = true, IsAccepted = true },
                new Purchase { GuitarName = "C40", IsProcessed = false }
            });

            _context.SaveChanges();

            _controller = new PurchasesController(_context, _mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAllPurchases()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Purchase>;
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public async Task Details_ReturnsPurchase_WhenIdExists()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Purchase;
            Assert.AreEqual("C40", model.GuitarName);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
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
            var purchase = new Purchase { DateTime = DateTime.Now };

            // Act
            var result = await _controller.Create(purchase);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);

            // Verify the purchase was added
            Assert.AreEqual(4, _context.Purchase.Count());
        }

        [Test]
        public async Task Create_ReturnsView_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Test error");
            var purchase = new Purchase();

            // Act
            var result = await _controller.Create(purchase);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(purchase, viewResult.Model);
        }

        [Test]
        public async Task Edit_ReturnsPurchase_WhenIdExists()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Purchase;
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
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
            var purchase = await _context.Purchase.FindAsync(1);
            purchase.DateTime = DateTime.Now.AddDays(-1);

            // Act
            var result = await _controller.Edit(1, purchase);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var updatedPurchase = await _context.Purchase.FindAsync(1);
            Assert.AreEqual(purchase.DateTime, updatedPurchase.DateTime);
        }

        [Test]
        public async Task Delete_ReturnsPurchase_WhenIdExists()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as Purchase;
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesPurchaseAndRedirects()
        {
            // Arrange
            var initialCount = _context.Purchase.Count();

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual(initialCount - 1, _context.Purchase.Count());
            Assert.IsNull(await _context.Purchase.FindAsync(1));
        }

        [Test]
        public async Task ProcessPurchases_UpdatesAcceptedPurchasesAndAvailability()
        {
            // Arrange
            var data = new PurchasesController.ProcessPurchasesData
            {
                AcceptedIds = new List<int> { 1 },
                DeclinedIds = new List<int> { 3 }
            };

            var initialAvailability = _context.Guitar.First(g => g.Name == "C40").Availability;

            // Act
            var result = await _controller.ProcessPurchases(data);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);

            var acceptedPurchase = await _context.Purchase.FindAsync(1);
            Assert.IsTrue(acceptedPurchase.IsProcessed);
            Assert.IsTrue(acceptedPurchase.IsAccepted);

            var declinedPurchase = await _context.Purchase.FindAsync(3);
            Assert.IsTrue(declinedPurchase.IsProcessed);
            Assert.IsFalse(declinedPurchase.IsAccepted);

            var updatedAvailability = _context.Guitar.First(g => g.Name == "C40").Availability;
            Assert.AreEqual(initialAvailability - 1, updatedAvailability);
        }
    }
}