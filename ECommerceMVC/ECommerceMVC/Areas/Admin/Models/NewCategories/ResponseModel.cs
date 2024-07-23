namespace ECommerceMVC.UI.Areas.Admin.Models.NewCategories
{
    public class ResponseModel
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; } = new object { };
        public object[] errors { get; set; } = new object[] { };

        public IEnumerable<string> EnumErrors { get; set; } = new string[] { };
    }
}
