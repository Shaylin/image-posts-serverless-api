using ImagePosts.Domain.Incoming.Queries;
using ImagePosts.Domain.Models;
using ImagePosts.Domain.Outgoing.Responses;
using Mediator;

namespace ImagePosts.Application.Incoming.Queries;

public class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, GetPostsResponse>
{
    public async ValueTask<GetPostsResponse> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        return new GetPostsResponse
        {
            Posts = new List<Post>(),
            NumberRemaining = 44,
            EndToken = "Fake"
        };
    }
}