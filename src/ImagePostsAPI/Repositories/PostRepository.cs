using System.Web;
using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Responses;

namespace ImagePostsAPI.Repositories;

public class PostRepository(
    IDynamoDBContext context,
    ILogger<IPostRepository> logger,
    ICommentRepository commentRepository) : IPostRepository
{
    public async Task<bool> CreatePost(Post post)
    {
        try
        {
            await context.SaveAsync(post);
            logger.LogInformation("Post {} is added", post.PostId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to persist {} to posts table", post);
            return false;
        }

        return true;
    }

    public async Task<PostsResponse> GetPosts(string? paginationToken = null, int limit = 10)
    {
        var scanConfig = new ScanOperationConfig
        {
            Limit = limit,
            PaginationToken = paginationToken
        };

        var response = context.FromScanAsync<Post>(scanConfig);

        var results = await response.GetRemainingAsync();

        return await GetPostsResponse(response.PaginationToken, results);
    }

    private async Task<PostsResponse> GetPostsResponse(string postCursor, List<Post> posts)
    {
        var postResponses = new List<PostResponse>();

        foreach (var post in posts)
        {
            var comments = await commentRepository.GetComments(post.PostId);
            var sortedComments = comments.OrderByDescending(x => x.CommentId).ToList();

            postResponses.Add(new PostResponse()
            {
                PostId = post.PostId,
                Caption = post.Caption,
                CreatedAt = post.CreatedAt,
                Creator = post.Creator,
                ImageUrl = $"https://{Environment.GetEnvironmentVariable("IMAGE_BUCKET")}.s3.{RegionEndpoint.AFSouth1.SystemName}.amazonaws.com/{HttpUtility.UrlEncode(post.ImagePath)}",
                LastComments = sortedComments
            });
        }

        var sortedResponses = postResponses.OrderByDescending(x => x.LastComments.Count).ToList();

        return new PostsResponse
        {
            Posts = sortedResponses,
            PostCursor = postCursor
        };
    }
}