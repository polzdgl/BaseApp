using BaseApp.Data.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.User.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

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
            };
        }
    }
}
