using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace ECommerceMVC.Helper.Facebooks
{
    public class FacebookHelper
    {
        public static async Task<string> GetPicture(string userID, string accessToken)
        {
            var pictureDefault = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(accessToken)) {
                return pictureDefault;
            }

            var httpClient = new HttpClient();          
            var response = await httpClient.GetAsync($"https://graph.facebook.com/{userID}?fields=picture.type(large)&access_token={accessToken}");

            if (!response.IsSuccessStatusCode) {
                return pictureDefault;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var userData = JObject.Parse(jsonResponse);
            return userData?["picture"]?["data"]?["url"]?.ToString() ?? pictureDefault;          
        }
    }
}
