using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Models.Role
{
    public class ModelCreateRole
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
    }
}
