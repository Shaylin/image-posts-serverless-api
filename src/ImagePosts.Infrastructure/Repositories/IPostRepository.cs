using ImagePostsAPI.Entities;

namespace ImagePosts.Infrastructure.Repositories;

public interface IPostRepository
{
    Task<bool> CreatePost(PostEntity postEntity);

    Task<List<PostEntity>> GetPosts(string? paginationToken = null, int limit = 10);
}