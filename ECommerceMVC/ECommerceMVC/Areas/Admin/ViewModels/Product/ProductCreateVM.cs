using ECommerceMVC.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.UI.Areas.Admin.ViewModels.Product
{
    public class ProductCreateVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a Category.")]
        public int IdCategory { get; set; }

        [Required(ErrorMessage = "Please select a Group.")]
        public int IdGroup { get; set; }

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be a positive number.")]
        public double Price { get; set; }

        public string Description { get; set; }

        [DateRange(nameof(RemovedAt), ErrorMessage = "CreatedAt must be earlier than RemovedAt.")]
        public DateTime CreatedAt { get; set; }

        [DateGreaterThan(nameof(CreatedAt), ErrorMessage = "RemovedAt must be later than CreatedAt.")]
        public DateTime RemovedAt { get; set; }

        [AtLeastOneItem(ErrorMessage = "Please select at least one color.")]
        public List<string> Colors { get; set; } = new List<string>();

        [AtLeastOneItem(ErrorMessage = "Please select at least one materials.")]
        public List<string> Materials { get; set; } = new List<string>();

        //[AtLeastOneImage(ErrorMessage = "Please select at least one image.")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        [AtLeastOneItem(ErrorMessage = "Please select at least one size.")]
        public List<string> Sizes { get; set; } = new List<string>();

        [BindNever]
        public List<string>? UrlImages { get; set; } = new List<string>();
    }
}
