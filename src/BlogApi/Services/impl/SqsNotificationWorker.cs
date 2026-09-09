
using Amazon.SQS;
using Amazon.SQS.Model;

namespace BlogApi.Services.impl;

public class SqsNotificationWorker : BackgroundService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;
    private readonly ILogger<SqsNotificationWorker> _logger;

    public SqsNotificationWorker(IAmazonSQS sqsClient, IConfiguration configuration, ILogger<SqsNotificationWorker> logger)
    {
        _sqsClient = sqsClient;
        _queueUrl = configuration["Aws:Sqs:NotificacoesQueueUrl"]!;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 5,
                WaitTimeSeconds = 10
            }, stoppingToken);

            if(response.Messages != null)
            {
                foreach (var message in response.Messages)
                {
                    _logger.LogInformation("Message received: {MessageBody}", message.Body);
                    await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle, stoppingToken);
                }
            }
        }
    }
}
