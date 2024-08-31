using ImagePostsAPI.Entities;

namespace ImagePostsAPI.Responses;

public class PostResponse : Post
{
    public required List<Comment> LastComments { get; set; }
}