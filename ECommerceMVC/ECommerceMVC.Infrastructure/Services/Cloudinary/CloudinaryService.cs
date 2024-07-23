using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerceMVC.Infrastructure.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ECommerceMVC.Domain.Abstract.Cloudinary;



namespace ECommerceMVC.Infrastructure.Services.Cloudinary
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
                throw new ArgumentNullException(nameof(file), "No file uploaded.");

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

        public async Task<(string Id, string Url)> UploadFileAsync(IFormFile file, string folder = "ECommerceMVC")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file), "No file uploaded.");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return (uploadResult.PublicId, uploadResult.SecureUrl.AbsoluteUri);
            }
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }

        public async Task<(string Id, string Url)> UpdateImageAsync(IFormFile newFile, string oldPublicId, string folder = "ECommerceMVC")
        {
            // Xóa ảnh cũ
            var deleteResult = await DeleteImageAsync(oldPublicId);
            if (deleteResult.Result != "ok")
                throw new Exception("Failed delete image id: " + oldPublicId);

            // Tải lên ảnh mới
            var uploadResult = await UploadFileAsync(newFile, folder);
            return uploadResult;
        }
    }
}
