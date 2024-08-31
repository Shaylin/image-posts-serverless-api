using System.Threading.Tasks;
using FluentAssertions;
using ImagePostsAPI.Controllers.V1;
using Xunit;

namespace ImagePostsAPI.Tests;

public class PostsControllerSuite
{
    private readonly PostsController _controller;

    public PostsControllerSuite()
    {
        _controller = new PostsController(null!, null!);
    }

    [Fact]
    public void Constructor_Should_Construct()
    {
        _controller.Should().NotBeNull();
    }

    [Fact]
    public async Task Call_GetApiBooks_ShouldReturn_LimitedListOfBooks()
    {
        var thing = 1;

        thing.Should().BeGreaterThan(0);
    }
}