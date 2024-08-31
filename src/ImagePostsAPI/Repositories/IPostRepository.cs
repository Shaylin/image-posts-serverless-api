using ImagePostsAPI.Entities;
using ImagePostsAPI.Responses;

namespace ImagePostsAPI.Repositories;

public interface IPostRepository
{
    Task<bool> CreatePost(Post post);

    Task<PostsResponse> GetPosts(string? startKey = null, int limit = 10);
}