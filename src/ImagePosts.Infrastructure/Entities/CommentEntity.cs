using Amazon.DynamoDBv2.DataModel;

namespace ImagePostsAPI.Entities;

[DynamoDBTable("comments")]
public class CommentEntity
{
    [DynamoDBHashKey]
    public required string CommentId { get; set; }
    
    [DynamoDBGlobalSecondaryIndexHashKey]
    public required string PostId { get; set; }

    public required string Content { get; set; }

    public required string Creator { get; set; }

    public required DateTime CreatedAt { get; set; }
}