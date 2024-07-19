using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.UI.Areas.Admin.Models.Role
{
    public class ModelCreateRole
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
    }
}
