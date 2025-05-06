

using NUnit.Framework;
using Moq;
using WebSearch.Core.Services;
using WebSearch.Core.Services.Models;
using global::WebSearch.Core.Services.Models;
using global::WebSearch.Core.Services;

namespace WebSearch.Tests.Services
{
    [TestFixture]
    public class SearchServiceTests
    {
        private Mock<SearchRequest> _mockSearchRequest;
        private SearchService _searchService;

        [SetUp]
        public void SetUp()
        {
            _mockSearchRequest = new Mock<SearchRequest>();
            _searchService = new SearchService();
        }

        [Test]
        public async Task Search_ShouldReturnExpectedResult()
        {
            // Arrange
            var url = "https://example.com";
            var keywords = "example";
            _mockSearchRequest.Setup(sr => sr.Search(url)).ReturnsAsync("Search result");

            // Act
            var result = await _searchService.Search(url, keywords, _mockSearchRequest.Object);

            // Assert
            Assert.Equals("Search result", result);
        }
    }
}
