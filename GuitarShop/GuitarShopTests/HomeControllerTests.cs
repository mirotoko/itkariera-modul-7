using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GuitarShop.Controllers;
using GuitarShop.Data;
using GuitarShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GuitarShopTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private GuitarShopContext _context;
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            // Configure in-memory database
            var options = new DbContextOptionsBuilder<GuitarShopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GuitarShopContext(options);

            // Seed test data
            _context.Guitar.AddRange(new List<Guitar>
            {
                new Guitar { Name = "C40", Body = "4/4" },
                new Guitar { Name = "Les Paul", Body = "Single Cut" },
                new Guitar { Name = "D-300", Body = "Dreadnought" },
                new Guitar { Name = "C10", Body = "4/4" }
            });
            _context.SaveChanges();

            _controller = new HomeController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsViewWithDistinctBodyTypes()
        {
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<string>>(result.Model);

            var bodyTypes = result.Model as List<string>;
            Assert.AreEqual(3, bodyTypes.Count); 
            Assert.Contains("4/4", bodyTypes);
            Assert.Contains("Single Cut", bodyTypes);
            Assert.Contains("Dreadnought", bodyTypes);
        }

        [Test]
        public async Task Index_ReturnsEmptyList_WhenNoGuitarsExist()
        {
            // Arrange - clear all guitars
            _context.Guitar.RemoveRange(_context.Guitar);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var bodyTypes = result.Model as List<string>;
            Assert.IsEmpty(bodyTypes);
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Guitars_ReturnsViewResult()
        {
            // Act
            var result = _controller.Guitars();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Shops_ReturnsViewResult()
        {
            // Act
            var result = _controller.Shops();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Error_ReturnsViewResultWithModel()
        {
            // Arrange - simulate HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ErrorViewModel>(result.Model);

            var model = result.Model as ErrorViewModel;
            Assert.IsNotNull(model.RequestId);
        }

        [Test]
        public void Error_ReturnsCorrectRequestId()
        {
            // Arrange
            var expectedRequestId = "test-request-id";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    TraceIdentifier = expectedRequestId
                }
            };

            // Act
            var result = _controller.Error() as ViewResult;
            var model = result.Model as ErrorViewModel;

            // Assert
            Assert.AreEqual(expectedRequestId, model.RequestId);
        }
    }
}