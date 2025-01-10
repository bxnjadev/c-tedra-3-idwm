using System.Collections.Immutable;
using Catedra3.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Catedra3.Service;

public class CloudinaryImageService
{
    private const int MaxSize = 10 * 1024 * 1024;
    private const int MinSize = 0;

    private static readonly
        ImmutableArray<string> ExtensionAble = [".jpg", ".png"];

    private const int Height = 500;
    private const int Width = 500;
    private const string Crop = "fill";
    private const string Gravity = "face";
    private const string Folder = "ucn-store";

    private readonly IOptions<CloudinarySettings> _config;
    private readonly string _urlConnection;
    private Cloudinary _cloudinary;
    
    public CloudinaryImageService(IOptions<CloudinarySettings> config)
    {
        _config = config;
        Connect();
    }
    
    /// <summary>
    /// Connects to Cloudinary using the settings provided in the configuration.
    /// </summary>
    private void Connect()
    {
        Console.WriteLine("URL = " + _config.Value.Url);
        _cloudinary = new Cloudinary(_config.Value.Url);
    }

    /// <summary>
    /// Uploads an image to Cloudinary with transformations applied.
    /// Validates file size and extension before uploading.
    /// </summary>
    /// <param name="formFile">The image file to be uploaded.</param>
    /// <returns>ImageUploadResult containing the result of the upload operation.</returns>
    public async Task<ImageUploadResult> Upload(IFormFile formFile)
    {
        var result = new ImageUploadResult();
        var length = formFile.Length;
        var extension = Path.GetExtension(formFile.FileName);

        if (!(MinSize <= length && length <= MaxSize) ||
            !(ExtensionAble.Contains(extension)))
        {
            Console.WriteLine("ERROR");
            return result;
        }

        await using var stream = formFile.OpenReadStream();
        var parameters = new ImageUploadParams
        {
            File = new FileDescription(formFile.FileName, stream),
            Transformation = new Transformation()
                .Width(Width)
                .Height(Height)
                .Crop(Crop)
                .Gravity(Gravity),
            Folder = Folder
        };

        return await _cloudinary.UploadAsync(parameters);
    }
    
}