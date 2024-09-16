using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ImagePostsAPI.Entities;
using Microsoft.Extensions.Logging;

namespace ImagePosts.Infrastructure.Repositories;

public class PostRepository(
    IDynamoDBContext context,
    ILogger<IPostRepository> logger,
    ICommentRepository commentRepository) : IPostRepository
{
    public async Task<bool> CreatePost(PostEntity postEntity)
    {
        try
        {
            await context.SaveAsync(postEntity);
            logger.LogInformation("Post {} is added", postEntity.PostId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to persist {} to posts table", postEntity);
            return false;
        }

        return true;
    }

    public async Task<List<PostEntity>> GetPosts(string? paginationToken = null, int limit = 10)
    {
        var scanConfig = new ScanOperationConfig
        {
            Limit = limit,
            PaginationToken = paginationToken
        };

        var response = context.FromScanAsync<PostEntity>(scanConfig);

        var results = await response.GetRemainingAsync();

        return results;
    }
}