using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize);
        Task<UserDto> GetUserAsync(string id);
        Task<bool> CreateUserAsync(UserRequestDto userRequestDto);
        Task<bool> UpdateUserAsync(string id, UserRequestDto userRequestDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
