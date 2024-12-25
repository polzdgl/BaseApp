using BaseApp.Data.User.Dtos;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace BaseApp.Data.User.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public static ApplicationUser FromDto(UserRequestDto userRequestDto)
        {
            return new ApplicationUser
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
