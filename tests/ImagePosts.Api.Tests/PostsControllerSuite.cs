using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ImagePosts.Infrastructure.Repositories;
using ImagePostsAPI.Controllers.V1;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Requests;
using ImagePostsAPI.Responses;
using ImagePostsAPI.Services.Identifier;
using ImagePostsAPI.Services.ImageStorage;
using ImagePostsAPI.Services.TimeStamp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ImagePostsAPI.Tests;

public class PostsControllerTests
{
    private readonly PostsController _controller;
    private readonly IImageStorageService _imageStorageService;
    private readonly ISortableIdentifierService _identifierService;
    private readonly ITimeStampService _timeStampService;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public PostsControllerTests()
    {
        var logger = Substitute.For<ILogger<PostsController>>();
        _imageStorageService = Substitute.For<IImageStorageService>();
        _identifierService = Substitute.For<ISortableIdentifierService>();
        _timeStampService = Substitute.For<ITimeStampService>();
        _postRepository = Substitute.For<IPostRepository>();
        _commentRepository = Substitute.For<ICommentRepository>();

        _controller = new PostsController(logger, _imageStorageService, _identifierService, _timeStampService,
            _postRepository, _commentRepository);
    }

    // [Fact]
    // public async Task GetPosts_ShouldReturnOkWithPostsResponse()
    // {
    //     const string startKey = "startKey";
    //     const int limit = 10;
    //
    //     var expectedResponse = new PostsResponse
    //     {
    //         Posts = null!,
    //         PostCursor = "AAA"
    //     };
    //
    //     _postRepository.GetPosts(startKey, limit).Returns(expectedResponse);
    //
    //     var result = await _controller.GetPosts(startKey, limit);
    //
    //     result.Result.Should().BeOfType<OkObjectResult>();
    //     var okResult = result.Result as OkObjectResult;
    //     okResult?.Value.Should().Be(expectedResponse);
    // }
    //
    // [Fact]
    // public async Task CreatePost_ShouldReturnBadRequestWhenModelStateIsInvalid()
    // {
    //     _controller.ModelState.AddModelError("Caption", "Required");
    //
    //     var result = await _controller.CreatePost(new CreatePostRequest
    //     {
    //         Caption = "This Is A Caption",
    //         PostImage = null!,
    //         Creator = "Someone"
    //     });
    //
    //     result.Result.Should().BeOfType<BadRequestObjectResult>();
    // }
    //
    // [Fact]
    // public async Task CreatePost_ShouldReturn500WhenImagePathIsNull()
    // {
    //     var request = new CreatePostRequest
    //     {
    //         Caption = "Some Caption",
    //         Creator = "Some Creator",
    //         PostImage = Substitute.For<IFormFile>()
    //     };
    //
    //     _imageStorageService.ConvertAndStoreImage(Arg.Any<string>(), request.PostImage).Returns((string)null!);
    //
    //     var result = await _controller.CreatePost(request);
    //
    //     result.Result.Should().BeOfType<ObjectResult>();
    //     var objectResult = result.Result as ObjectResult;
    //     objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    // }
    //
    // [Fact]
    // public async Task CreatePost_ShouldReturnOkWhenPostIsSuccessfullyCreated()
    // {
    //     var request = new CreatePostRequest
    //     {
    //         Caption = "Caption",
    //         Creator = "Creator",
    //         PostImage = Substitute.For<IFormFile>()
    //     };
    //
    //     const string postId = "generatedPostId";
    //     const string imagePath = "imagePath";
    //     var post = new Post
    //     {
    //         PostId = postId,
    //         Caption = request.Caption,
    //         Creator = request.Creator,
    //         CreatedAt = System.DateTime.UtcNow,
    //         ImagePath = imagePath,
    //     };
    //
    //     _identifierService.Generate().Returns(postId);
    //     _imageStorageService.ConvertAndStoreImage(postId, request.PostImage).Returns(imagePath);
    //     _timeStampService.GetUtcNow().Returns(post.CreatedAt);
    //     _postRepository.CreatePost(Arg.Any<Post>()).Returns(true);
    //
    //     var result = await _controller.CreatePost(request);
    //
    //     result.Result.Should().BeOfType<OkObjectResult>();
    //     var okResult = result.Result as OkObjectResult;
    //     okResult?.Value.Should().BeEquivalentTo(post);
    // }
    //
    // [Fact]
    // public async Task CreateComment_ShouldReturnBadRequestWhenModelStateIsInvalid()
    // {
    //     _controller.ModelState.AddModelError("Content", "Required");
    //
    //     var result = await _controller.CreateComment("postId", new CreateCommentRequest
    //     {
    //         Content = null!,
    //         Creator = null!
    //     });
    //
    //     result.Result.Should().BeOfType<BadRequestObjectResult>();
    // }
    //
    //
    // [Fact]
    // public async Task CreateComment_ShouldReturnOkWhenCommentIsSuccessfullyCreated()
    // {
    //     const string postId = "postId";
    //     const string commentId = "commentId";
    //
    //     var request = new CreateCommentRequest
    //     {
    //         Content = "Test Content",
    //         Creator = "Test Creator"
    //     };
    //
    //     var comment = new Comment
    //     {
    //         CommentId = commentId,
    //         Content = request.Content,
    //         Creator = request.Creator,
    //         PostId = postId,
    //         CreatedAt = System.DateTime.UtcNow
    //     };
    //
    //     _identifierService.Generate().Returns(commentId);
    //     _timeStampService.GetUtcNow().Returns(comment.CreatedAt);
    //     _commentRepository.CreateComment(Arg.Any<Comment>()).Returns(true);
    //
    //     var result = await _controller.CreateComment(postId, request);
    //
    //     result.Result.Should().BeOfType<OkObjectResult>();
    //     var okResult = result.Result as OkObjectResult;
    //     okResult?.Value.Should().BeEquivalentTo(comment);
    // }
    //
    // [Fact]
    // public async Task DeleteComment_ShouldReturnNotFoundWhenCommentDoesNotExist()
    // {
    //     const string postId = "postId";
    //     const string commentId = "commentId";
    //     var request = new DeleteCommentRequest { Creator = "Creator" };
    //
    //     _commentRepository.GetComment(commentId).Returns((Comment)null!);
    //
    //     var result = await _controller.DeleteComment(postId, commentId, request);
    //
    //     result.Should().BeOfType<NotFoundResult>();
    // }
    //
    // [Fact]
    // public async Task DeleteComment_ShouldReturnUnauthorizedWhenCreatorDoesNotMatch()
    // {
    //     const string postId = "anotherPostId";
    //     const string commentId = "anotherCommentId";
    //
    //     var existingComment = new Comment
    //     {
    //         Creator = "Different Creator",
    //         PostId = postId,
    //         CommentId = commentId,
    //         Content = "SOME OTHER CREATOR MADE THIS",
    //         CreatedAt = default
    //     };
    //     var request = new DeleteCommentRequest { Creator = "Creator" };
    //
    //     _commentRepository.GetComment(commentId).Returns(existingComment);
    //
    //     var result = await _controller.DeleteComment(postId, commentId, request);
    //
    //     result.Should().BeOfType<UnauthorizedResult>();
    // }
    //
    // [Fact]
    // public async Task DeleteComment_ShouldReturnOkWhenCommentIsSuccessfullyDeleted()
    // {
    //     const string postId = "postId";
    //     const string commentId = "commentId";
    //
    //     var existingComment = new Comment
    //     {
    //         Creator = "Creator",
    //         PostId = postId,
    //         CommentId = commentId,
    //         Content = postId,
    //         CreatedAt = default
    //     };
    //
    //     var request = new DeleteCommentRequest { Creator = "Creator" };
    //
    //     _commentRepository.GetComment(commentId).Returns(existingComment);
    //
    //     var result = await _controller.DeleteComment(postId, commentId, request);
    //
    //     result.Should().BeOfType<OkResult>();
    //     await _commentRepository.Received(1).DeleteComment(commentId);
    // }
}