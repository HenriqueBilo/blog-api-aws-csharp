using Amazon.DynamoDBv2.DataModel;

namespace BlogApi.Models;

[DynamoDBTable("blog-api-posts")]
public class Post
{
	[DynamoDBHashKey]
	public string PostId { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorSub {  get; set; }
    public string? ImageUrl { get; set; }
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

}

public record CreatePostRequest(string Title, string Content);
public record PostResponse(string PostId, string Title, string Content, string AuthorSub, string? ImageUrl, DateTime CreatedAtUtc);
