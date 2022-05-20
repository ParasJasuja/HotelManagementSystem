using System.ComponentModel.DataAnnotations;
namespace HotelManagementSystem.Utility
{
    public class ValidValueAttribute : ValidationAttribute
    {
        private string[] _allowedValues;

        public ValidValueAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_allowedValues.Contains(value))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Not a valid Room Type");
        }
    }
}
