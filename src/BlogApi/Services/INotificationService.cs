using BlogApi.Models;

namespace BlogApi.Services;

public interface INotificationService
{
    Task PublishNewCommentAsync(Comment comment);
}
