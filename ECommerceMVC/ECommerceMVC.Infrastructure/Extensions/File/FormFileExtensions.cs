

using EECommerceMVC.Infrastructure.Helpers.File;
using Microsoft.AspNetCore.Http;

namespace ECommerceMVC.Infrastructure.Extensions.File
{
    public static class FormFileExtensions
    {
        public static async Task<string> ToBase64String(this IFormFile file)
        {
            return await FileHelper.GetFileBase64Async(file);
        }
    }
}
