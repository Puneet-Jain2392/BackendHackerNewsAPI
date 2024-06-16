using HackerAPI.Models;
namespace HackerAPI.Repositories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetNewestStoriesAsync();
    }
}
