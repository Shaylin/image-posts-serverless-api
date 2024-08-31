
namespace ImagePostsAPI.Responses;

public class PostsResponse
{
    public required List<PostsResponse> Posts { get; set; }

    public string? PostCursor { get; set; }
}