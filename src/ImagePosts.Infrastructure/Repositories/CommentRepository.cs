using Amazon.DynamoDBv2.DataModel;
using ImagePostsAPI.Entities;
using Microsoft.Extensions.Logging;

namespace ImagePosts.Infrastructure.Repositories;

public class CommentRepository(IDynamoDBContext context, ILogger<ICommentRepository> logger) : ICommentRepository
{
    public async Task<bool> CreateComment(CommentEntity commentEntity)
    {
        try
        {
            await context.SaveAsync(commentEntity);
            logger.LogInformation("Post {} is added", commentEntity.CommentId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to persist {} to comments table", commentEntity);
            return false;
        }

        return true;
    }

    public Task<List<CommentEntity>> GetComments(string postId)
    {
        var results = context.QueryAsync<CommentEntity>(postId, new DynamoDBOperationConfig()
        {
            IndexName = "PostIdIndex"
        });

        return results.GetRemainingAsync();
    }

    public Task<CommentEntity?> GetComment(string commentId)
    {
        try
        {
            return context.LoadAsync<CommentEntity>(commentId)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to retrieve {} from comments table", commentId);
            return null!;
        }
    }

    public async Task DeleteComment(string commentId)
    {
        await context.DeleteAsync<CommentEntity>(commentId);
    }
}