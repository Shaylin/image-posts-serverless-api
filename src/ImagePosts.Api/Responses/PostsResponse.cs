
namespace ImagePostsAPI.Responses;

public class PostsResponse
{
    public required List<PostResponse> Posts { get; set; }

    public string? PostCursor { get; set; }
}