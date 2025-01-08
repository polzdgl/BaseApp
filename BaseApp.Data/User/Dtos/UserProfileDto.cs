using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Dtos
{
    public record UserProfileDto
    {
        [Required(ErrorMessage = "User Name is required!")]
        public string UserName => Email;

        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "12/31/2100", ErrorMessage = "Date of Birth must be valid.")]
        public DateTime? DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
