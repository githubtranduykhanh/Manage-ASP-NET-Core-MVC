using ECommerceMVC.Helper.File;

namespace ECommerceMVC.Extension.File
{
    public static class FormFileExtensions
    {
        public static async Task<string> ToBase64String(this IFormFile file)
        {
            return await FileHelper.GetFileBase64Async(file);
        }
    }
}
