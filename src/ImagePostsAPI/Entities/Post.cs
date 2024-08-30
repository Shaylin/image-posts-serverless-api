using Amazon.DynamoDBv2.DataModel;

namespace ImagePostsAPI.Entities;

[DynamoDBTable("posts")]
public class Post
{
    [DynamoDBHashKey]
    public string PostId { get; set; }

    public string Caption { get; set; }

    public string ImagePath { get; set; }

    public string Creator { get; set; }
}