using BaseApp.Data.User.Dtos;
using FluentValidation;

namespace BaseApp.Data.User.Validator
{
    internal sealed class UserRegistrationDtoValidator : AbstractValidator<UserRegisterDto>
    {
        int minimumAge = 18;

        public UserRegistrationDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Password)            
                .NotEmpty().WithMessage("Password is required.")       
                .MinimumLength(12).WithMessage("Password must be at least 12 characters long.")       
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")             
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")           
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")               
                .Matches(@"[\W_]").WithMessage("Password must contain at least one non-alphanumeric character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match!");

            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => dateOfBirth == null || IsValidAge(dateOfBirth.Value, minimumAge)).WithMessage($"You must me atleast {minimumAge} years old to register!");

            RuleFor(x => x.PhoneNumber).Matches(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$").WithMessage("Phone number is not valid");
        }

        private bool IsValidAge(DateTime dateOfBirth, int minimumAge)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age >= minimumAge;
        }
    }
}