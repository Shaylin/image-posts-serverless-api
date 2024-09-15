namespace ImagePostsAPI.Services.ImageStorage;

public interface IImageStorageService
{
    Task<string?> ConvertAndStoreImage(string prefix, IFormFile imageFile);
}