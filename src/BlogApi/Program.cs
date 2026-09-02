using Amazon.DynamoDBv2;
using Amazon.S3;
using BlogApi.Services;
using BlogApi.Services.impl;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["Aws:Region"])));
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["Aws:Region"])));
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
