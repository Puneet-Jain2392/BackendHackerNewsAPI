using HackerAPI.Models;
using HackerAPI.Repositories;
using HackerAPI.Services;
using Moq;
using Xunit;
namespace HackerAPI.Tests
{
    public class StoryServiceTests
    {
   
        private readonly Mock<IStoryRepository> _mockRepository;
        private readonly IStoryService _storyService;

        public StoryServiceTests()
        {
            _mockRepository = new Mock<IStoryRepository>();
            _storyService = new StoryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetNewestStoriesAsync_ShouldReturnStories()
        {
            // Arrange
            var stories = new List<Story>
            {
                new Story { id = 1, title = "Story 1", url = "http://story1.com" },
                new Story { id = 2, title = "Story 2", url = "http://story2.com" }
            };
            _mockRepository.Setup(repo => repo.GetNewestStoriesAsync()).ReturnsAsync(stories);

            // Act
            var result = await _storyService.GetNewestStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Story>)result).Count);
        }
    }
}
