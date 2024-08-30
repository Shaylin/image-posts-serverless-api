using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ImagePostsAPI.Repositories;


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
    .AddScoped<IDynamoDBContext, DynamoDBContext>()
    .AddScoped<IBookRepository, BookRepository>();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();