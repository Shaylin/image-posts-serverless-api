using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.DynamoDBv2.DataModel;
using ImagePostsAPI.Repositories;
using ImagePostsAPI.Services.Encoding;
using ImagePostsAPI.Services.Identifier;
using ImagePostsAPI.Services.ImageStorage;
using ImagePostsAPI.Services.TimeStamp;


var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddJsonConsole();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.AFSouth1.SystemName;
builder.Services
    .AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region)))
    .AddSingleton<IAmazonS3>(new AmazonS3Client(RegionEndpoint.GetBySystemName(region)))
    .AddSingleton<IJpegEncoderService, ImageSharpJpegEncoderService>()
    .AddSingleton<IImageStorageService, S3ImageStorageService>()
    .AddSingleton<ISortableIdentifierService, UlidSortableIdentifierService>()
    .AddSingleton<ITimeStampService, TimeStampService>()
    .AddScoped<IDynamoDBContext, DynamoDBContext>()
    .AddScoped<ICommentRepository, CommentRepository>()
    .AddScoped<IPostRepository, PostRepository>();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();