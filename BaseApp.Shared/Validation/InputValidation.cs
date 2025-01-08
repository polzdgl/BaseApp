using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BaseApp.Shared.Validation
{
    public class InputValidation
    {
        // Validate a model using Data Annotations
        public bool ValidateModel<T>(T model, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            return Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        }

        // Validate Password with predefined rules
        public bool ValidatePassword(string password, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var passwordRequirements = new PasswordRequirements();
            var result = passwordRequirements.ValidatePassword(password);

            if (result.Any())
            {
                validationResults = result;
                return false;
            }

            return true;
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

        public bool ValidateDateOfBirth(object value, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            // Check if the value is a valid DateTime or DateOnly
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.Date <= DateTime.Today)
                {
                    return true;
                }

                validationErrorMessage = "The date of birth cannot be in the future.";
                return false;
            }
            else if (value is DateOnly dateOnlyValue)
            {
                if (dateOnlyValue <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return true;
                }

                validationErrorMessage = "The date of birth cannot be in the future.";
                return false;
            }

            validationErrorMessage = "The value must be a valid date.";
            return false;
        }


        // Validate a phone number
        public bool ValidatePhoneNumber(string phoneNumber, out string validationErrorMessage)
        {
            string phonePattern = @"^(\+?\d{1,3})?[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";
            if (Regex.IsMatch(phoneNumber, phonePattern))
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The phone number is not in a valid format.";
            return false;
        }

        // Validate an email address
        public bool ValidateEmailAddress(string email, out string validationErrorMessage)
        {
            string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            if (Regex.IsMatch(email, emailPattern))
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The email address is not in a valid format.";
            return false;
        }

        public bool ValidateUserName(string userName, out string validationErrorMessage)
        {
            if (RegexHelper.IsOnlyLettersNumbers(userName))
            {
                validationErrorMessage = string.Empty;
                return true;
            }

            validationErrorMessage = "The UserName is not in a valid format. Please enter letters and numbers only!";
            return false;
        }

        // Get validation errors as a formatted string
        public string GetValidationErrors(List<ValidationResult> validationResults)
        {
            return string.Join(Environment.NewLine, validationResults.Select(vr => vr.ErrorMessage));
        }


    }
}
