using ImagePosts.Application;
using ImagePosts.Domain.Incoming.Requests;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace ImagePostsAPI.Controllers.V1;

[Route("v1/[controller]")]
[Produces("application/json")]
public class PostsController(
    IMediator mediator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetPosts(
        [FromQuery] string? startToken,
        [FromQuery] int limit = 10
    )
    {
        var request = new GetPostsRequest { StartToken = startToken, Limit = limit };

        var result = await mediator.Send(request.ToGetPostsQuery());

        return Ok(result);
    }
}