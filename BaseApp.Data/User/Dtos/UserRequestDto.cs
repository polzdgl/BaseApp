namespace BaseApp.Data.User.Dtos
{
    public record UserRequestDto
    {        
        public required string UserName { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
