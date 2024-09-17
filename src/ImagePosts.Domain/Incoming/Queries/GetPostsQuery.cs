using ImagePosts.Domain.Models;
using Mediator;

namespace ImagePosts.Domain.Incoming.Queries;

public record GetPostsQuery : IQuery<IEnumerable<Post>>
{
    public string? StartToken { get; init; }

    public int Limit { get; init; }
}