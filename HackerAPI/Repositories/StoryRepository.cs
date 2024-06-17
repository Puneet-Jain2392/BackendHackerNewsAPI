using HackerAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerAPI.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public StoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Story>> GetNewestStoriesAsync()
        {
            try
            {
                var apiUrl = _configuration["APIUrl"];
                var newStoriesUrl = $"{apiUrl}v0/newstories.json";

                // Fetch story ids
                var storyIdsResponse = await _httpClient.GetAsync(newStoriesUrl);
                storyIdsResponse.EnsureSuccessStatusCode();
                var storyIds = await JsonSerializer.DeserializeAsync<IEnumerable<int>>(
                    await storyIdsResponse.Content.ReadAsStreamAsync());

                // Fetch stories
                var tasks = storyIds.Select(async id =>
                {
                    var storyUrl = $"{apiUrl}v0/item/{id}.json";
                    var storyResponse = await _httpClient.GetAsync(storyUrl);
                    storyResponse.EnsureSuccessStatusCode();
                    var storyContent = await storyResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Story>(storyContent);
                });

                var stories = (await Task.WhenAll(tasks))
                                  .Where(story => story != null && !string.IsNullOrEmpty(story.url))
                                  .ToList();

                return stories;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error fetching stories from Hacker News API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred in StoryRepository.", ex);
            }
        }

        // Dispose HttpClient properly when no longer needed
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
