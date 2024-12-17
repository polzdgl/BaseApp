using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BaseApp.Shared.Validation
{
    public class InputValidation
    {
        // Method to validate the model
        public bool ValidateModel<T>(T model, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);
            return isValid;
        }

        // Method to validate a numeric value within a specific range
        public bool ValidateNumberRange(int number, int minValue, int maxValue, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Check if the number is within the range
            if (number < minValue || number > maxValue)
            {
                validationErrorMessage = $"The number must be between {minValue} and {maxValue}.";
                return false;
            }

            return true;
        }

        // Method to validate if a number is a valid numeric value (int or double)
        public bool ValidateNumber(object value, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Check if the value is a number (int or double)
            if (value is int || value is double || value is float || value is decimal)
            {
                return true;
            }

            validationErrorMessage = "The value must be a valid numeric value.";
            return false;
        }

        // Method to validate if a date is a valid DateTime or DateOnly
        public bool ValidateDate(object value, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Check if the value is a valid DateTime or DateOnly
            if (value is DateTime || value is DateOnly)
            {
                return true;
            }

            validationErrorMessage = "The value must be a valid date.";
            return false;
        }

        // Method to validate if a phone number is in a valid format (e.g., +1234567890)
        public bool ValidatePhoneNumber(string phoneNumber, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Use regular expressions to validate the phone number pattern
            string phonePattern = @"^(\+\d{1,3}|\d{1,4})?[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";
            if (Regex.IsMatch(phoneNumber, phonePattern))
            {
                return true;
            }

            validationErrorMessage = "The phone number is not in a valid format.";
            return false;
        }

        // Method to validate if an email address is in a valid format
        public bool ValidateEmailAddress(string email, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Use regular expressions to validate the email address pattern
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (Regex.IsMatch(email, emailPattern))
            {
                return true;
            }

            validationErrorMessage = "The email address is not in a valid format.";
            return false;
        }

        // Method to get validation errors as a string
        public string GetValidationErrors(List<ValidationResult> validationResults)
        {
            return string.Join("\n", validationResults.Select(vr => vr.ErrorMessage));
        }

    }
}
