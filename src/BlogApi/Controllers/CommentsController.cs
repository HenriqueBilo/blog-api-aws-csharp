using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("posts/{postId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentsService;

    public CommentsController(ICommentService commentService)
    {
        _commentsService = commentService;

    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> ListByPostIdAsync(string postId)
    {
        var commentsList = await _commentsService.ListByPostIdAsync(postId);

        return Ok(commentsList);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateAsync(CreateCommentRequest request, string postId)
    {
        var comment = new Comment()
        {
            Text = request.Text,
            PostId = postId,
            AuthorSub = "test"
        };


        var commentCreated = await _commentsService.CreateAsync(comment);

        return Ok(commentCreated);
    }
}
