using ImagePosts.Domain.Models;

namespace ImagePosts.Domain.Outgoing.Responses;

public class GetPostsResponse
{
    public required IEnumerable<Post> Posts { get; init; }

    public string? EndToken { get; init; }

    public int? NumberRemaining { get; init; }
}