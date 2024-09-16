using ImagePostsAPI.Entities;

namespace ImagePostsAPI.Responses;

public class PostResponse
{
    public required string PostId { get; set; }

    public required string Caption { get; set; }

    public required string ImageUrl { get; set; }

    public required string Creator { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required List<CommentEntity> LastComments { get; set; }
}