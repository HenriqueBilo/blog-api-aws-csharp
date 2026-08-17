using BlogApi.Models;

namespace BlogApi.Services;

public interface IPostService
{
    Task<Post> CreateAsync(Post post);
    Task<Post?> GetByIdAsync(string postId);
    Task<List<Post>> ListAllAsync();
}
