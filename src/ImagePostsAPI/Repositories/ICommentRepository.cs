using ImagePostsAPI.Entities;

namespace ImagePostsAPI.Repositories;

public interface ICommentRepository
{
    Task<bool> CreateComment(Comment comment);

    Task<List<Comment>> GetComments(string postId);

    Task DeleteComment(string commentId);
}