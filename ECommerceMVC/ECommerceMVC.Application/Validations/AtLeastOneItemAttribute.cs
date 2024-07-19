using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Validation
{
    public class AtLeastOneItemAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as List<string>;

            if (list == null || list.Count == 0 || list.Any(item => item == null))
            {
                return new ValidationResult(ErrorMessage ?? "Please select at least one item.");
            }

            

            return ValidationResult.Success;
        }
    }
}
