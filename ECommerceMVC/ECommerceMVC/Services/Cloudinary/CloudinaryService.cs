using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using ECommerceMVC.Config;
using Microsoft.Extensions.Options;

namespace ECommerceMVC.Services.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
       
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var settings = cloudinarySettings.Value;
            Account account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret);

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded.");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "ECommerceMVC", // Thư mục trên Cloudinary để lưu file
                    Overwrite = true // Ghi đè nếu file đã tồn tại
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.SecureUrl.AbsoluteUri; // Trả về URL an toàn để truy cập file
            }
        }
    }
}
