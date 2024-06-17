using HackerAPI.Caching;
using HackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ICacheService _cacheService;

        public StoriesController(IStoryService storyService, ICacheService cacheService)
        {
            _storyService = storyService;
            _cacheService = cacheService;
        }

        [HttpGet("newest")]
        public async Task<IActionResult> GetNewestStories()
        {
            try
            {
                var stories = await _cacheService.GetOrCreateAsync("newest_stories", () => _storyService.GetNewestStoriesAsync());
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}