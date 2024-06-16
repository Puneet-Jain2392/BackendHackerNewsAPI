using HackerAPI.Models;
using System.Text.Json;

namespace HackerAPI.Repositories
{


    public class StoryRepository : IStoryRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public StoryRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        

        public async Task<IEnumerable<Story>> GetNewestStoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_configuration["APIUrl"]}v0/newstories.json");
            response.EnsureSuccessStatusCode();

            var storyIds = JsonSerializer.Deserialize<IEnumerable<int>>(await response.Content.ReadAsStringAsync());

            var stories = new List<Story>();
            foreach (var id in storyIds)
            {
                var storyResponse = await _httpClient.GetAsync($"{_configuration["APIUrl"]}v0/item/{id}.json");
                storyResponse.EnsureSuccessStatusCode();
                var story = JsonSerializer.Deserialize<Story>(await storyResponse.Content.ReadAsStringAsync());
                if (story != null && !string.IsNullOrEmpty(story.url))
                {
                    stories.Add(story);
                }
            }

            return stories;
        }
    }
}
