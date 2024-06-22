namespace ECommerceMVC.Config
{
    public class GoogleSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
    }
    public class FacebookSettings
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string CallbackPath { get; set; }
    }

    public class AuthenticationSettings
    {
        public GoogleSettings Google { get; set; }
        public FacebookSettings Facebook { get; set; }
    }

}