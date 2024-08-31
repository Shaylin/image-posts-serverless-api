using System.ComponentModel.DataAnnotations;

namespace ImagePostsAPI.Requests;

public class DeleteCommentRequest
{
    [Required]
    public required string Creator { get; set; }
}