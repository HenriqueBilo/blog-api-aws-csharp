namespace BlogApi.Models.Events;

public record NewCommentEvent(string EventType, string CommentId, string PostId, string AuthorSub, DateTime CreatedAtUtc);
