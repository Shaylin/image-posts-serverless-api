using ImagePosts.Domain.Models;

using Mediator;

namespace ImagePosts.Domain.Incoming.Requests;

public struct GetPostsRequest: IRequest<List<Post>>
{
    
}