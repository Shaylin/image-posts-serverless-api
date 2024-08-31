using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using ImagePostsAPI.Services.Encoding;
using ImagePostsAPI.Services.ImageStorage;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace ImagePostsAPI.Tests.Services.ImageStorage;

public class S3ImageStorageServiceSuite
{
    private readonly S3ImageStorageService _s3ImageStorageService;
    private readonly IAmazonS3 _mockS3;
    private readonly IJpegEncoderService _mockJpegEncoderService;

    public S3ImageStorageServiceSuite()
    {
        Environment.SetEnvironmentVariable("IMAGE_BUCKET", "myBucketName");

        _mockS3 = Substitute.For<IAmazonS3>();
        _mockJpegEncoderService = Substitute.For<IJpegEncoderService>();

        _s3ImageStorageService = new S3ImageStorageService(_mockS3, _mockJpegEncoderService);
    }

    [Fact]
    public void Constructor_Should_Construct()
    {
        _s3ImageStorageService.Should().NotBeNull();
    }

    [Fact]
    public async Task ConvertAndStoreImage_ShouldConvertMemoryStreamToJpegUsingEncoderService()
    {
        _mockS3.PutObjectAsync(Arg.Any<PutObjectRequest>()).Returns(new PutObjectResponse());

        var fakeStream = new MemoryStream();

        var fakeFormFile = new FormFile(
            fakeStream,
            0,
            0,
            "test",
            "test.jpg"
        );

        await _s3ImageStorageService.ConvertAndStoreImage("something", fakeFormFile);

        await _mockJpegEncoderService.Received().Encode(fakeFormFile);
    }

    [Fact]
    public async Task ConvertAndStoreImage_StoreEncodedMemoryStreamInS3()
    {
        _mockS3.PutObjectAsync(Arg.Any<PutObjectRequest>()).Returns(new PutObjectResponse
            { HttpStatusCode = System.Net.HttpStatusCode.OK });

        var fakeStream = new MemoryStream();
        var expectedConvertedStream = new MemoryStream();

        _mockJpegEncoderService.Encode(Arg.Any<IFormFile>()).Returns(expectedConvertedStream);

        var fakeFormFile = new FormFile(
            fakeStream,
            0,
            0,
            "test",
            "test.png"
        );

        var returnedPath = await _s3ImageStorageService.ConvertAndStoreImage("somePath", fakeFormFile);

        await _mockS3.Received().PutObjectAsync(Arg.Is<PutObjectRequest>(req =>
            req.BucketName == Environment.GetEnvironmentVariable("IMAGE_BUCKET") &&
            req.Key == "somePath/test.jpeg" &&
            req.ContentType == "image/jpeg"
        ));

        returnedPath.Should().Be("somePath/test.jpeg");
    }
}