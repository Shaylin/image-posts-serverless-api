using ImagePosts.Domain.Incoming.Queries;
using ImagePosts.Domain.Models;
using Mediator;

namespace ImagePosts.Application.Incoming.Queries;

public class GetPostsQueryHandler: IQueryHandler<GetPostsQuery, IEnumerable<Post>>
{
    public ValueTask<IEnumerable<Post>> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}