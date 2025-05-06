using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebSearchApi.Controllers;
using WebSearch.Core.Models;
using WebSearch.DataAccess;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebSearch.Tests
{
    [TestFixture]
    public class SearchApiControllerTests
    {
        private Mock<WebSearchDbContext> _mockDbContext;
        private Mock<ILogger<SearchApiController>> _mockLogger;
        private SearchApiController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<WebSearchDbContext>();
            _mockLogger = new Mock<ILogger<SearchApiController>>();
            _controller = new SearchApiController(_mockDbContext.Object, _mockLogger.Object);
        }

        [Test]
        public void GetAllSearches_ShouldReturnSearches()
        {
            // Arrange
            var searches = new List<Search>
            {
                new Search { SearchId = 1, Url = "https://example.com", KeyWords = "example" },
                new Search { SearchId = 2, Url = "https://another.com", KeyWords = "another" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Search>>();
            mockSet.As<IQueryable<Search>>().Setup(m => m.Provider).Returns(searches.Provider);
            mockSet.As<IQueryable<Search>>().Setup(m => m.Expression).Returns(searches.Expression);
            mockSet.As<IQueryable<Search>>().Setup(m => m.ElementType).Returns(searches.ElementType);
            mockSet.As<IQueryable<Search>>().Setup(m => m.GetEnumerator()).Returns(searches.GetEnumerator());

            _mockDbContext.Setup(db => db.Searches).Returns(mockSet.Object);

            // Act
            var result = _controller.GetAllSearches();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(2, ((IEnumerable<Search>)okResult.Value).Count());
        }
    }
}
