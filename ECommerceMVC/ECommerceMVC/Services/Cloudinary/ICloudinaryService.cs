using CloudinaryDotNet.Actions;

namespace ECommerceMVC.Services.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<(string Id, string Url)> UploadFileAsync(IFormFile file, string folder = "ECommerceMVC");
        Task<string> UploadFileAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
        Task<(string Id, string Url)> UpdateImageAsync(IFormFile newFile, string oldPublicId, string folder = "ECommerceMVC");
    }
}
