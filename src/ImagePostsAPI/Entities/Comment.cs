using Amazon.DynamoDBv2.DataModel;

namespace ImagePostsAPI.Entities;

[DynamoDBTable("comments")]
public class Comment
{
    [DynamoDBHashKey]
    public string CommentId { get; set; }
    
    [DynamoDBGlobalSecondaryIndexHashKey]
    public string PostId { get; set; }

    public string Content { get; set; }

    public string Creator { get; set; }

    public DateTime CreatedAt { get; set; }
}