using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BlogApi.Models;
using BlogApi.Models.Events;
using System.Text.Json;

namespace BlogApi.Services.impl;

public class NotificationService : INotificationService
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly string _topicArn;

    public NotificationService(IAmazonSimpleNotificationService amazonSimpleNotificationService, IConfiguration configuration)
    {
        _snsClient = amazonSimpleNotificationService;
        _topicArn = configuration["Aws:Sns:NewCommentTopicArn"]!;
    }

    public async Task PublishNewCommentAsync(Comment comment)
    {
        var newCommentEvent = new NewCommentEvent
        (
            "NewComment",
            comment.CommentId,
            comment.PostId,
            comment.AuthorSub,
            comment.CreatedAtUtc
        );

        var payload = JsonSerializer.Serialize(newCommentEvent);

        await _snsClient.PublishAsync(new PublishRequest
        {
            TopicArn = _topicArn,
            Message = payload
        });

    }
}
