namespace ECommerceMVC.Helper.Responses
{
    public class AuthenticationFailedResponse
    {
        public bool status { get; set; } = false;
        public string mes { get; set; } = string.Empty;
        public object[] data { get; set; } = new object[] {};
    }
}
