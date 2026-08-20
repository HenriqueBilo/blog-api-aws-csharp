using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BlogApi.Models;

namespace BlogApi.Services.impl;

public class CommentService : ICommentService
{
    private readonly DynamoDBContext _context;
    private readonly string _tableName;

    public CommentService(IAmazonDynamoDB amazonDynamoDb, IConfiguration configuration)
    {
        _context = new DynamoDBContextBuilder().WithDynamoDBClient(() => amazonDynamoDb).Build();
        _tableName = configuration["Aws:DynamoDb:CommentsTable"]!;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var config = new SaveConfig()
        {
            OverrideTableName = _tableName
        };

        await _context.SaveAsync(comment, config);

        return comment;
    }

    public async Task<List<Comment>> ListByPostIdAsync(string postId)
    {
        var config = new QueryConfig()
        {
            OverrideTableName = _tableName,
            IndexName = "PostId-Index"
        };

        var search = _context.QueryAsync<Comment>(postId, config);

        var results = new List<Comment>();

        while(!search.IsDone)
        {
            var page = await search.GetNextSetAsync();
            results.AddRange(page);
        }

        return results;
    }
}
