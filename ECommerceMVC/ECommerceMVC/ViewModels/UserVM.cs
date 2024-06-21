
using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(12, ErrorMessage = "Phone cannot be longer than 12 characters")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; } = "123456";

        public int IdRole { get; set; } = 2;

        [Required(ErrorMessage = "Sex is required")]
        [StringLength(10)]
        [Display(Name = "Sex")] 
        public string Sex { get; set; } = "other";

        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "LoginType is required")]
        [StringLength(50)]
        [Display(Name = "Login Type")]
        public string LoginType { get; set; } = "normally";

        public string RefreshToken { get; set; }

        public string Avatar { get; set; } = "avatadefaul.png";

        [Display(Name = "Security Question")]
        public string SecurityQuestion { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
