namespace ECommerceMVC.Helper.File
{
    public class FileHelper
    {
        public static async Task<byte[]> GetFileBytesAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<string> GetFileBase64Async(IFormFile file)
        {
            var fileBytes = await GetFileBytesAsync(file);
            return Convert.ToBase64String(fileBytes);
        }
    }
}
