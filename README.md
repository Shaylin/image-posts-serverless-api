# Image Posts Serverless API

![image posts design](./ImagePostsDesign.png)

## Build Requirements

* AWS CLI - [Install the AWS CLI](https://aws.amazon.com/cli/)
* SAM
  CLI - [Install the SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html)
* .NET Core 8 - [Install .NET Core](https://www.microsoft.com/net/download)
* Docker - [Install Docker community edition](https://hub.docker.com/search/?type=edition&offering=community)

## Building

```bash
sam build
```  

## Testing Locally

An emulated API can be run locally with the following command:

```bash
sam local start-api --env-vars dev_environment_variables.json
```

Where dev_environment_variables.json is in the format:

```json
{
  "ImagePostsHandler": {
    "POSTS_TABLE": "posts",
    "COMMENT_TABLE": "comment",
    "IMAGE_BUCKET": "<insert the name of the s3 bucket to test with>"
  }
}
```

Note that the AWS CLI needs to be configured on your system with credentials to access DynamoDB tables and S3 buckets on
your cloud account.

## Deploying

```bash
sam deploy --guided
```

## Design Decisions

- AWS SAM
- API Gateway Multi-Locations
- MultiPart IFormFile For Splitting Images
- Dynamo DB
-

## Remaining Tasks Before Production

### Implement IdGen Snowflake Identifiers

### Utilize A Relational Database

- This is better handled in the DB engine
- If we want to go this route, we need to limit responses to a certain time range with a FilterExpression
- Foreign key constraints

### Implement More Robust Validation

### Implement Integration Testing

### Consider CloudFront CDN

### Consider A CQRS-Style Pattern

### Consider Retry Middleware

