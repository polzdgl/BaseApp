using BaseApp.Data.User.Dtos;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task<bool> AddUserAsync(UserRequestDto userRequestDto);
        Task<bool> UpdateUserAsync(string id, UserRequestDto userRequestDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
