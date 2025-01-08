using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseApp.Shared.Validation
{
    public class PasswordRequirements : ValidationAttribute
    {
        /// <summary>
        /// Validates the password according to predefined rules.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>A list of validation results indicating any violations.</returns>
        public List<ValidationResult> ValidatePassword(string password)
        {
            var validationResults = new List<ValidationResult>();

            if (string.IsNullOrEmpty(password))
            {
                validationResults.Add(new ValidationResult("Password is required."));
            }

            if (password.Length < 12)
            {
                validationResults.Add(new ValidationResult("Password must be at least 12 characters long."));
            }

            if (!Regex.IsMatch(password, @"\d")) // Require digit
            {
                validationResults.Add(new ValidationResult("Password must contain at least one digit."));
            }

            if (!Regex.IsMatch(password, @"[a-z]")) // Require lowercase
            {
                validationResults.Add(new ValidationResult("Password must contain at least one lowercase letter."));
            }

            if (!Regex.IsMatch(password, @"[A-Z]")) // Require uppercase
            {
                validationResults.Add(new ValidationResult("Password must contain at least one uppercase letter."));
            }

            if (!Regex.IsMatch(password, @"[\W_]")) // Require non-alphanumeric
            {
                validationResults.Add(new ValidationResult("Password must contain at least one non-alphanumeric character."));
            }

            return validationResults;
        }
    }
}
