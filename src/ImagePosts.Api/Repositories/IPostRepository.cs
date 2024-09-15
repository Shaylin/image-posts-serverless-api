using ImagePostsAPI.Entities;
using ImagePostsAPI.Responses;

namespace ImagePostsAPI.Repositories;

public interface IPostRepository
{
    Task<bool> CreatePost(Post post);

    Task<PostsResponse> GetPosts(string? paginationToken = null, int limit = 10);
}