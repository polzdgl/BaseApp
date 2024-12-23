using System.ComponentModel;

namespace BaseApp.Data.User.Dtos
{
    public record UserRequestDto
    {        
        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
