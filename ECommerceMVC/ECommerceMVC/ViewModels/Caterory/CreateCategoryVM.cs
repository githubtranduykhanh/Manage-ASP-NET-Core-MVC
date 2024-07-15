using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels.Caterory
{
    public class CreateCategoryVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int? IdCategoryParent { get; set; }

        [Required(ErrorMessage = "Display Order is required")]
        public int DisplayOrder { get; set; }
    }
}
