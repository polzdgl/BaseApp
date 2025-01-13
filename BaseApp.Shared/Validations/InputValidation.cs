using System.ComponentModel.DataAnnotations;

namespace BaseApp.Shared.Validations
{
    // Helper class to validate input data
    public class InputValidation
    {
        // Validate a model using Data Annotations
        public bool ValidateModel<T>(T model, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            return Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        }

        // Validate if a number is within a specific range
        public bool ValidateNumberRange(int number, int minValue, int maxValue, out string validationErrorMessage)
        {
            if (number >= minValue && number <= maxValue)
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = $"The number must be between {minValue} and {maxValue}.";
            return false;
        }

        // Validate if a value is numeric (int, float, double, decimal)
        public bool ValidateNumber(object value, out string validationErrorMessage)
        {
            if (value is int or float or double or decimal)
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The value must be a valid numeric value.";
            return false;
        }

        // Validate ID (greater than 0)
        public bool ValidateId(string value, out string validationErrorMessage)
        {
            if (Guid.TryParse(value, out _))
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The ID must be a valid GUID.";
            return false;
        }

        // Validate if a value is a valid DateTime or DateOnly
        public bool ValidateDate(object value, out string validationErrorMessage)
        {
            if (value is DateTime or DateOnly)
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The value must be a valid date.";
            return false;
        }

        // Validate if input list is empty or in a specific range
        public bool ValidateIntList(IEnumerable<int> values, int minValue, int maxValue, out string validationErrorMessage)
        {
            if (values == null || !values.Any())
            {
                validationErrorMessage = "The list cannot be null or empty.";
                return false;
            }

            if (values.Any(value => value < minValue || value > maxValue))
            {
                validationErrorMessage = $"All integers in the list must be between {minValue} and {maxValue}.";
                return false;
            }

            validationErrorMessage = string.Empty;
            return true;
        }

        // Get validation errors as a formatted string
        public string GetValidationErrors(List<ValidationResult> validationResults)
        {
            return string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage));
        }
    }
}
