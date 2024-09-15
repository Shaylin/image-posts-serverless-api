namespace ImagePostsAPI.Services.Encoding;

public interface IJpegEncoderService
{
    Task<MemoryStream> Encode(IFormFile imageFile);
}