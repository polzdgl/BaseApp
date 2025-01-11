using BaseApp.Shared.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.User.Dtos
{
    public record UserRegisterDto : IValidatableObject
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        public string UserName => Email; 

        [Range(typeof(DateTime), "1/1/1900", "12/31/2100", ErrorMessage = "Date of Birth must be valid.")]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Confirm Password is required!")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Passwords do not match!", new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
