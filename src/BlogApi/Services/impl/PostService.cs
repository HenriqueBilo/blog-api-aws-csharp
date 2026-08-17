using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BlogApi.Models;

namespace BlogApi.Services.impl;

public class PostService : IPostService
{
    private readonly DynamoDBContext _context;
    private readonly string _tableName;

    public PostService(IAmazonDynamoDB amazonDynamoDB, IConfiguration configuration)
    {
        _context = new DynamoDBContextBuilder().WithDynamoDBClient(() => amazonDynamoDB).Build();
        _tableName = configuration["Aws:DynamoDb:PostsTable"]!;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        var dynamoConfig = new SaveConfig()
        {
            OverrideTableName = _tableName
        };

        await _context.SaveAsync(post, dynamoConfig);

        return post;
    }

    public async Task<Post?> GetByIdAsync(string postId)
    {
        var dynamoConfig = new LoadConfig()
        {
            OverrideTableName = _tableName
        };

        return await _context.LoadAsync<Post>(postId, dynamoConfig);
    }

    public async Task<List<Post>> ListAllAsync()
    {
        var dynamoConfig = new ScanConfig()
        {
            OverrideTableName = _tableName
        };

        var search = _context.ScanAsync<Post>(new List<ScanCondition>(), dynamoConfig);
        var results = new List<Post>();

        while(!search.IsDone)
        {
            var page = await search.GetNextSetAsync();
            results.AddRange(page);
        }

        return results;
    }
}
