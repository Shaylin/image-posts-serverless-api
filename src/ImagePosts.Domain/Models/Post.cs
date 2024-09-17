namespace ImagePosts.Domain.Models;

public class Post
{
    public required string Id { get; init; }

    public required string Caption { get; init; }

    public required string ImageUrl { get; init; }

    public required string Creator { get; init; }

    public required DateTime CreatedAt { get; init; }
}