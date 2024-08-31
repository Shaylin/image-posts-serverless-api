using ImagePostsAPI.Entities;

namespace ImagePostsAPI.Repositories;

public interface ICommentRepository
{
    Task<bool> CreateComment(Comment comment);

    Task<List<Comment>> GetComments(string postId);

    Task<Comment?> GetComment(string commentId);
    
    Task DeleteComment(string commentId);
}