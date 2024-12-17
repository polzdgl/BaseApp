using BaseApp.Data.User.Dtos;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> AddUserAsync(UserRequestDto userRequestDto);
        Task<bool> UpdateUserAsync(int id, UserRequestDto userRequestDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
