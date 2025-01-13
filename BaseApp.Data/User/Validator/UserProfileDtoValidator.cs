using BaseApp.Data.User.Dtos;
using FluentValidation;

namespace BaseApp.Data.User.Validator
{
    internal sealed class UserProfileDtoValidator : AbstractValidator<UserProfileDto>
    {
        int minimumAge = 18;

        // This is a class that validates the UserProfileDto using fluent validation
        public UserProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

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
