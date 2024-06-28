using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Models.Role
{
    public class ModelEditRole
    {
        [Required(ErrorMessage = "Role Id is required")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
    }
}
