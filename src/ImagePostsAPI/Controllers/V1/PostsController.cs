using System.ComponentModel.DataAnnotations;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Repositories;
using ImagePostsAPI.Requests;
using ImagePostsAPI.Responses;
using ImagePostsAPI.Services.Identifier;
using ImagePostsAPI.Services.ImageStorage;
using ImagePostsAPI.Services.TimeStamp;
using Microsoft.AspNetCore.Mvc;

namespace ImagePostsAPI.Controllers.V1;

[Route("v1/[controller]")]
[Produces("application/json")]
public class PostsController(
    ILogger<PostsController> logger,
    IBookRepository bookRepository,
    IImageStorageService imageStorageService,
    ISortableIdentifierService identifierService,
    ITimeStampService timeStampService,
    IPostRepository postRepository,
    ICommentRepository commentRepository)
    : ControllerBase
{
    private readonly IBookRepository _bookRepository = bookRepository;

    [HttpGet]
    public async Task<ActionResult<PostsResponse>> GetPosts([FromQuery] string? startKey, [FromQuery] int limit = 10)
    {
        logger.LogInformation("Get posts request");

        return Ok(new Post()
        {
            PostId = "a",
            Caption = "b",
            Creator = "c",
            ImagePath = "c",
            CreatedAt = DateTime.Now
        });
    }

    [RequestSizeLimit(100 * 1024 * 1024)]
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromForm] [Required] CreatePostRequest createPostRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var postId = identifierService.Generate();

        var imagePath = await imageStorageService.ConvertAndStoreImage(postId, createPostRequest.PostImage);

        if (imagePath is null) return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload image");

        var post = new Post()
        {
            PostId = postId,
            Caption = createPostRequest.Caption,
            Creator = createPostRequest.Creator,
            CreatedAt = timeStampService.GetUtcNow(),
            ImagePath = imagePath,
        };

        var postSuccessfullyWritten = await postRepository.CreatePost(post);

        return !postSuccessfullyWritten
            ? StatusCode(StatusCodes.Status500InternalServerError, "Failed to persist post")
            : Ok(post);
    }

    [HttpPost("{postId}/comments")]
    public async Task<ActionResult<Comment>> CreateComment([FromRoute] string postId,
        [FromBody] CreateCommentRequest createCommentRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var commentId = identifierService.Generate();

        var comment = new Comment()
        {
            CommentId = commentId,
            Content = createCommentRequest.Content,
            CreatedAt = timeStampService.GetUtcNow(),
            Creator = createCommentRequest.Creator,
            PostId = postId
        };

        var commentSuccessfullyWritten = await commentRepository.CreateComment(comment);

        return !commentSuccessfullyWritten
            ? StatusCode(StatusCodes.Status500InternalServerError, "Failed to persist comment")
            : Ok(commentId);
    }

    [HttpDelete("{postId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] string postId, [FromRoute] string commentId,
        [FromBody] [Required] DeleteCommentRequest deleteCommentRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok();
    }
}