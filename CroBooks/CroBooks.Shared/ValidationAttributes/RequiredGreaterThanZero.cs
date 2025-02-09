using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.ValidationAttributes
{
    public class RequiredGreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.DisplayName} is required");
            }
            // return true if value is a non-null number > 0, otherwise return false
            decimal i;
            var isValid = value != null && decimal.TryParse(value.ToString(), out i) && i > 0;

            if (!isValid)
                return new ValidationResult($"The field {validationContext.DisplayName} must be greater than 0");

            return ValidationResult.Success;
        }
    }
}
