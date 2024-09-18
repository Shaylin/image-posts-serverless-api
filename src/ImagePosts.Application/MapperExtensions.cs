using ImagePosts.Domain.Incoming.Queries;
using ImagePosts.Domain.Incoming.Requests;
using Riok.Mapperly.Abstractions;

namespace ImagePosts.Application;

[Mapper]
public static partial class MapperExtensions
{
    public static partial GetPostsQuery ToGetPostsQuery(this GetPostsRequest request);
}