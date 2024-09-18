using ImagePosts.Domain.Models;
using Mediator;

namespace ImagePosts.Domain.Incoming.Requests;

public record GetPostsRequest : IRequest<List<Post>>
{
    public string? StartToken { get; init; }

    public int Limit { get; init; }
}