using HackerAPI.Controllers;
using HackerAPI.Models;
using HackerAPI.Services;
using HackerAPI.Caching;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HackerAPI.Tests
{
    public class StoriesControllerTests
    {
   
        private readonly Mock<IStoryService> _mockService;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly StoriesController _controller;

        public StoriesControllerTests()
        {
            _mockService = new Mock<IStoryService>();
            _mockCacheService = new Mock<ICacheService>();
            _controller = new StoriesController(_mockService.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task GetNewestStories_ShouldReturnOkResult_WithListOfStories()
        {
            // Arrange
            var stories = new List<Story>
            {
                new Story { id = 1, title = "Story 1", url = "http://story1.com" },
                new Story { id = 2, title = "Story 2", url = "http://story2.com" }
            };
            _mockCacheService.Setup(cache => cache.GetOrCreateAsync("newest_stories", It.IsAny<Func<Task<IEnumerable<Story>>>>()))
                .ReturnsAsync(stories);

            // Act
            var result = await _controller.GetNewestStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
