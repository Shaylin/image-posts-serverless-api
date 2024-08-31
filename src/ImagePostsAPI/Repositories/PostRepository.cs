using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Responses;

namespace ImagePostsAPI.Repositories;

public class PostRepository(IDynamoDBContext context, ILogger<IPostRepository> logger) : IPostRepository
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

    public async Task<PostsResponse> GetPosts(string? startKey = null, int limit = 10)
    {
        var allPosts = new List<Post>();

        var scanConfig = new ScanOperationConfig
        {
            Limit = limit,
            PaginationToken = startKey
        };

        var response = context.FromScanAsync<Post>(scanConfig);

        do
        {
            var results = await response.GetNextSetAsync();
            allPosts = (List<Post>)allPosts.Concat(results);
        } while (response.IsDone == false && allPosts.Count < limit);

        return await GetPostsResponse(response.PaginationToken, allPosts);
    }

    private async Task<PostsResponse> GetPostsResponse(string postCursor, List<Post> posts)
    {
        return null;
    }
}