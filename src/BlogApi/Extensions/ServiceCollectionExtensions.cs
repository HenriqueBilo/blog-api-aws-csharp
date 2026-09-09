using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using BlogApi.Services;
using BlogApi.Services.impl;

namespace BlogApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAwsClients(this IServiceCollection services, IConfiguration configuration)
    {
        var awsRegion = Amazon.RegionEndpoint.GetBySystemName(configuration["Aws:Region"]);

        services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(awsRegion));
        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(awsRegion));
        services.AddSingleton<IAmazonCognitoIdentityProvider>(_ => new AmazonCognitoIdentityProviderClient(awsRegion));
        services.AddSingleton<IAmazonSimpleNotificationService>(_ => new AmazonSimpleNotificationServiceClient(awsRegion));
        services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient(awsRegion));
        

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHostedService<SqsNotificationWorker>();

        return services;
    }
}
