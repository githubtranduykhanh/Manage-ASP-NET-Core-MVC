using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Models
{
    public class UserStore
    {
        public bool Status { get; set; } = false;
        public UserVM? UserVM { get; set; } = null;
    }
}
