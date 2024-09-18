using Amazon.S3;
using Amazon.S3.Model;
using ImagePostsAPI.Services.Encoding;

namespace ImagePostsAPI.Services.ImageStorage;

public class S3ImageStorageService(IAmazonS3 s3, IJpegEncoderService jpegEncoderService) : IImageStorageService
{
    public async Task<string?> ConvertAndStoreImage(string prefix, IFormFile imageFile)
    {
        var encodedImage = await jpegEncoderService.Encode(imageFile);

        var imagePath = $"{prefix}/{Path.GetFileNameWithoutExtension(imageFile.FileName)}.jpeg";

        var uploadRequest = new PutObjectRequest
        {
            InputStream = encodedImage,
            BucketName = Environment.GetEnvironmentVariable("IMAGE_BUCKET"),
            Key = imagePath,
            ContentType = "image/jpeg"
        };

        var response = await s3.PutObjectAsync(uploadRequest);

        return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? imagePath : null;
    }
}