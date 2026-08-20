using BlogApi.Models;

namespace BlogApi.Services;

public interface ICommentService
{
    Task<Comment> CreateAsync(Comment comment);
    Task<List<Comment>> ListByPostIdAsync(string postId);
}
