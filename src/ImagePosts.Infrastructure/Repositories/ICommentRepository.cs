using ImagePostsAPI.Entities;

namespace ImagePosts.Infrastructure.Repositories;

public interface ICommentRepository
{
    Task<bool> CreateComment(CommentEntity commentEntity);

    Task<List<CommentEntity>> GetComments(string postId);

    Task<CommentEntity?> GetComment(string commentId);
    
    Task DeleteComment(string commentId);
}