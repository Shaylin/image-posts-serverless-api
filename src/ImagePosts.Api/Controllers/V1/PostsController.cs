using ImagePosts.Domain.Incoming.Requests;
using ImagePostsAPI.Responses;
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
    public async Task<ActionResult<PostsResponse>> GetPosts(
        [FromQuery] string? paginationToken,
        [FromQuery] int limit = 10
    )
    {
        var request = new GetPostsRequest { StartToken = paginationToken, Limit = limit };

        var result = await mediator.Send(request);

        return Ok(result);
    }
}