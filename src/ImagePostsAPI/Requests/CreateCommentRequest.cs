using System.ComponentModel.DataAnnotations;

namespace ImagePostsAPI.Requests;

public class CreateCommentRequest
{
    [Required]
    public required string Content { get; set; }

    [Required]
    public required string Creator { get; set; }
}