using Amazon.DynamoDBv2.DataModel;

namespace ImagePostsAPI.Entities;

[DynamoDBTable("posts")]
public class Post
{
    [DynamoDBHashKey]
    public required string PostId { get; set; }

    public required string Caption { get; set; }

    public required string ImagePath { get; set; }

    public required string Creator { get; set; }
    
    public required DateTime CreatedAt { get; set; }
}