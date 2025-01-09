using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.User.Dtos
{
    public record UserProfileDto
    {
        public string UserName => Email;

        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
