using BaseApp.Data.User.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace BaseApp.Data.User.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public static User FromDto(UserRequestDto userRequestDto)
        {
            return new User
            {
                UserName = userRequestDto.UserName,
                Email = userRequestDto.Email,
                PhoneNumber = userRequestDto.PhoneNumber,
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                DateOfBirth = userRequestDto.DateOfBirth,
                IsActive = userRequestDto.IsActive,
            };
        }
    }
}
