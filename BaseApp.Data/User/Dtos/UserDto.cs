using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.User.Dtos
{
    public record UserDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "User Name is required!")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required!")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public required string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "12/31/2100", ErrorMessage = "Date of Birth must be valid.")]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool IsActive { get; set; }

        public static UserDto FromModel(Models.User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IsActive = user.IsActive,
            };
        }

        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue)
                {
                    return null;
                }

                var today = DateTime.Today;
                var birthDate = DateOfBirth.Value;
                var age = today.Year - birthDate.Year;

                if (birthDate > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }
    }
}
