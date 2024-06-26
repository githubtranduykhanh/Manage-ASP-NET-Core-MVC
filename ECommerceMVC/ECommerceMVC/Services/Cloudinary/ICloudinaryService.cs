namespace ECommerceMVC.Services.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
