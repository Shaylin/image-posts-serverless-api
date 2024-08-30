using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ImagePostsAPI.Tests;

public class PostsControllerSuite
{
    private readonly WebApplicationFactory<Program> _webApplication;
    public PostsControllerSuite()
    {
        _webApplication = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Mock the repository implementation
                    //to remove infra dependencies for Test project
                    services.AddScoped<IBookRepository, MockBookRepository>();
                });
            });
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Call_GetApiBooks_ShouldReturn_LimitedListOfBooks(int limit)
    {
        var client = _webApplication.CreateClient();
        var books = await client.GetFromJsonAsync<IList<Book>>($"/api/Books?limit={limit}");

        Assert.NotEmpty(books);
        Assert.Equal(limit, books?.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task Call_GetApiBook_ShouldReturn_BadRequest(int limit)
    {
        var client = _webApplication.CreateClient();
        var result = await client.GetAsync($"/api/Books?limit={limit}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, result?.StatusCode);

    }
}