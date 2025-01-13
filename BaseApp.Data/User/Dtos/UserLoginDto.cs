using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.User.Dtos
{
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the user wants to be remembered.
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}
