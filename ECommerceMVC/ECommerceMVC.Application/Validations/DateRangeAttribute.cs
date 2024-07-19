using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateRangeAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (propertyInfo == null)
            {
                return new ValidationResult($"Unknown property {_comparisonProperty}");
            }

            var comparisonValue = propertyInfo.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (value is DateTime date && comparisonValue.HasValue)
            {
                if (date >= comparisonValue.Value)
                {
                    return new ValidationResult(ErrorMessage ?? $"The {validationContext.MemberName} must be earlier than {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
