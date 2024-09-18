using System.ComponentModel.DataAnnotations;

namespace ImagePostsAPI.Requests;

public class CreatePostRequest
{
    [Required]
    public required string Caption { get; set; }

    [Required]
    public required IFormFile PostImage { get; set; }

    [Required]
    public required string Creator { get; set; }
}