using Amazon.DynamoDBv2.DataModel;

namespace BlogApi.Models;

[DynamoDBTable("blog-api-comments")]
public class Comment
{
    [DynamoDBHashKey]
    public string CommentId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBGlobalSecondaryIndexHashKey("PostId-Index")]
    public string PostId { get; set; }

    public string AuthorSub { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public record CreateCommentRequest(string Text);

public record CommentResponse(string CommentId, string PostId, string AuthorSub, string Text, DateTime CreatedAtUtc);
