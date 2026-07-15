using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Prodemos.Application.Models.ImageManagement;
using Prodemos.Application.Services.Interfaces;
using System.Net;

namespace Prodemos.Infrastructure.Services;
public class ManageImageService : IManageImageService
{
    private readonly IConfiguration _config;

    public ManageImageService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task<ImageResponse> UploadImage(ImageData imageData)
    {
        Cloudinary cloudinary = new Cloudinary(_config["Cloudinary:CloudinaryUrl"]);
        cloudinary.Api.Secure = true;

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(imageData.Name, imageData.ImageStream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };
        var uploadResult = await cloudinary.UploadAsync(uploadParams);
        Console.WriteLine(uploadResult.JsonObj);

        if (uploadResult.StatusCode == HttpStatusCode.OK)
        {
            return new ImageResponse
            {
                publicId = uploadResult.PublicId,
                url = uploadResult.Url.ToString(),
            };
        }

        throw new Exception("Image could not be stored.");
    }
}
