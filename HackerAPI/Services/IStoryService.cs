using HackerAPI.Models;
namespace HackerAPI.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<Story>> GetNewestStoriesAsync();
    }
}
