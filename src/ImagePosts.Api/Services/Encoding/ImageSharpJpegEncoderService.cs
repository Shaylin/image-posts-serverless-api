using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImagePostsAPI.Services.Encoding;

public class ImageSharpJpegEncoderService : IJpegEncoderService
{
    public async Task<MemoryStream> Encode(IFormFile imageFile)
    {
        var image = await Image.LoadAsync(imageFile.OpenReadStream());

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(600, 600),
            Mode = ResizeMode.Max
        }));

        var memoryStream = new MemoryStream();
        await image.SaveAsJpegAsync(memoryStream, new JpegEncoder { Quality = 90 });

        return memoryStream;
    }
}