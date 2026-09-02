using Amazon.DynamoDBv2;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IAmazonDynamoDB _amazonDynamoDB;
    private readonly IImageService _imageService;

    public PostsController(IPostService postService, IAmazonDynamoDB amazonDynamoDB, IImageService imageService)
    {
        _postService = postService;
        _amazonDynamoDB = amazonDynamoDB;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> ListAllAync()
    {
        var posts = await _postService.ListAllAsync();

        return Ok(posts);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Post>> GetByIdAsync(string postId)
    {
        var post = await _postService.GetByIdAsync(postId);

        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreateAsync(CreatePostRequest request)
    {
        var newPost = new Post()
        {
            Title = request.Title,
            Content = request.Content,
            AuthorSub = "test"
        };

        var postCreated = await _postService.CreateAsync(newPost);

        return Ok(postCreated);
    }

    [HttpPost("{postId}/imagem")]
    public async Task<ActionResult> UploadImage(string postId, IFormFile file)
    {
        var post = await _postService.GetByIdAsync(postId);
        if (post == null)
            return NotFound();

        var imageKey = await _imageService.UploadAsync(file, postId);
        post.ImageUrl = imageKey;

        await _postService.CreateAsync(post);

        return Ok(new { imageKey });
    }
}
