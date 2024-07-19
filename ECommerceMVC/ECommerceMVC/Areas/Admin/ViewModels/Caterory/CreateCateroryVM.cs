using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.UI.Areas.Admin.ViewModels.Caterory
{
    public class CreateCateroryVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int? IdCategoryParent { get; set; }

        [Required(ErrorMessage = "Display Order is required")]
        public int DisplayOrder { get; set; }
    }
}
