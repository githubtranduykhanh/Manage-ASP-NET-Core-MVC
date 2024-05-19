using Newtonsoft.Json;

namespace ECommerceMVC.Helper.Responses
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteJsonResponseAsync<T>(this HttpResponse response, T responseObject, int statusCode,string mesHeader)
        {
            response.Headers.Append("WWW-Authenticate", $"Bearer error={mesHeader}");
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            var jsonResponse = JsonConvert.SerializeObject(responseObject);
            await response.WriteAsync(jsonResponse);
        }
    }
}
