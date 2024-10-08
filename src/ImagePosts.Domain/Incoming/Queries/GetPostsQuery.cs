using ImagePosts.Domain.Outgoing.Responses;
using Mediator;

namespace ImagePosts.Domain.Incoming.Queries;

public record GetPostsQuery : IQuery<GetPostsResponse>
{
    public string? StartToken { get; init; }

    public int Limit { get; init; }
}