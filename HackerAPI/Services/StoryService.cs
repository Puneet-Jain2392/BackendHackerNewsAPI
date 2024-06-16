using HackerAPI.Models;
using HackerAPI.Repositories;

namespace HackerAPI.Services
{

    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<IEnumerable<Story>> GetNewestStoriesAsync()
        {
            return await _storyRepository.GetNewestStoriesAsync();
        }
    }
}
