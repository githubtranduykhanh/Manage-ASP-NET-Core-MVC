using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.UI.Areas.Admin.ViewModels.Group
{
    public class CreateGroupVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int? IdGroupParent { get; set; }

        [Required(ErrorMessage = "Display Order is required")]
        public int DisplayOrder { get; set; }
    }
}
