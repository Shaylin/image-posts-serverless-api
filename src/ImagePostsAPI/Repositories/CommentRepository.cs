using Amazon.DynamoDBv2.DataModel;
using ImagePostsAPI.Entities;

namespace ImagePostsAPI.Repositories;

public class CommentRepository(IDynamoDBContext context, ILogger<ICommentRepository> logger) : ICommentRepository
{
    public async Task<bool> CreateComment(Comment comment)
    {
        try
        {
            await context.SaveAsync(comment);
            logger.LogInformation("Post {} is added", comment.CommentId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to persist {} to comments table", comment);
            return false;
        }

        return true;
    }

    public Task<List<Comment>> GetComments(string postId)
    {
        var results = context.QueryAsync<Comment>(postId, new DynamoDBOperationConfig()
        {
            IndexName = "PostIdIndex"
        });

        return results.GetRemainingAsync();
    }

    public Task<Comment?> GetComment(string commentId)
    {
        try
        {
            return context.LoadAsync<Comment>(commentId)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to retrieve {} from comments table", commentId);
            return null!;
        }
    }

    public async Task DeleteComment(string commentId)
    {
        await context.DeleteAsync<Comment>(commentId);
    }
}