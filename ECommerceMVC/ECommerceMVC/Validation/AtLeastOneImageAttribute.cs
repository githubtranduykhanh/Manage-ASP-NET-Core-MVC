using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Validation
{
    public class AtLeastOneImageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var images = value as List<IFormFile>;

            if (images == null || images.Count == 0)
            {
                return new ValidationResult("Please select at least one image.");
            }

            return ValidationResult.Success;
        }
    }
}
