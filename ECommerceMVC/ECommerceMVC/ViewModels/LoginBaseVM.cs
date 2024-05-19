using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels
{
    public class LoginBaseVM
    {
        [Required]
        public string emailorphone { get; set; } = string.Empty;
        [Required]
        public string passwordlogin { get; set; } = string.Empty;
    }
}
