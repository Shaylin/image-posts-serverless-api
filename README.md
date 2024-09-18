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

## Design & Features

### AWS SAM & CloudFormation IaC

SAM was chosen as an abstraction over CloudFormation as it offers the ability to spin up a serverless API without much
code.
It also streamlines the process of deploying by automatically handling the S3 bucket to hold templates.

### API Gateway

This was selected as it does span edge locations. This can help with the image upload process for users with poor
connections.

### Dynamo DB Tables

This was chosen purely as a cost and speed priority. It is actually a very poor fit for this application.
A relational database would be better as it would allow us to avoid sorting and filtering on the server.
Complex queries could instead be handled within the database engine.

### GitHub Actions Quality Checks

IaC building & validation were added as CI quality checks. While there aren't many unit tests due to time pressure, they
are also run in CI.

## Remaining Tasks Before Production

### Implement IdGen Snowflake Identifiers

Identifiers for comments and posts are currently generated ULIDs. This offers the uniqueness of GUID, but with the added
benefit of being sortable by time of creation.

This works well enough in low volume scenarios, however, there is always a small risk of concurrent lambda functions to
generate the same identifier.

For use social media applications like this, Snowflake Identifiers are preferred as they also include an identifier
unique to the runner in them.
In the case of ECS, this can be the IP address of the current runner.
Lambda requires a bit more effort in terms of leasing identifiers to each running lambda with something like a mutex
scheme.

### Utilize A Relational Database

As mentioned in the section above, a relational database is a much better fit for this task.
Dynamo DB has a limitation of 400KB per item, which means that all comments cannot reasonably be stored under a single
item.
This necessitated a separate table configured with a global secondary index.
While this works reasonably well for now, a relational database would allow for easier additions of new features.

### Implement More Robust Validation

While some minimal model validation exists, there is no deeper inspection on the validity of the values of the model.

Validators should be introduced in the pipeline to improve usability.

### Implement Integration Testing

There are minimal tests in this project. This should be expanded and integration tests should also be added to assure
quality as time goes on.

### Consider Dotnet 8 AOT

Standard dotnet is not good when it comes to cold start times.

However, there is the option to run ahead of time compilation to reduce package sizes and reduce cold starts
drastically.

This requires a bit of extra work in the controller, but should be considered as a performance optimization.

### Consider CloudFront CDN

Presently, images are served from a public S3 bucket in a single region.

CloudFront should be used to optimize the serving of static assets to multiple regions, with the ability to offer edge
caching.

### Consider Retry Middleware & Pre-Signed Upload URLs

While the usage of API gateway does help the upload situation a bit. The best solution for this would be to have retry
middleware to avoid data loss.

Additionally, a simpler way of handling user uploads would be to return a pre-signed S3 URL that can be handled within
the client.

This can be coupled with things like S3 multi-part upload and transfer acceleration.
