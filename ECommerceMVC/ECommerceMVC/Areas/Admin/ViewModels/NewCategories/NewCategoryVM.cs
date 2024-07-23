using ECommerceMVC.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.UI.Areas.Admin.ViewModels.NewCategories
{
    public class NewCategoryVM
    {      
        public int? Id { get; set; }

        [Required(ErrorMessage = "Parent category is required.")]
        public int IdNewParent { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Display Order must be a non-negative number.")]
        public int DisplayOrder { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile? Image { get; set; } // Nếu bạn cần upload file
        public string? UrlImage { get; set; } // Nếu bạn cần upload file

        public string? NewParentName { get; set; } = string.Empty;
        public List<DbNew>? DbNews { get; set; } = new List<DbNew>();
    }
}
