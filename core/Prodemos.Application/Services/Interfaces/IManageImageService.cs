using Prodemos.Application.Models.ImageManagement;

namespace Prodemos.Application.Services.Interfaces;
public interface IManageImageService
{
    Task<ImageResponse> UploadImage(ImageData imageData);
}
