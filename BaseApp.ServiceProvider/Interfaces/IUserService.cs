using BaseApp.Data.User.Dtos;
using BaseApp.Data.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> AddUserAsync(UserDto personDto);
        Task<bool> UpdateUserAsync(int id, UserDto personDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
