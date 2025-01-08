using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Interfaces.User
{
    public interface IUserService
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize);
        Task<UserDto> GetUserAsync(string id);
        Task<bool> CreateUserAsync(UserProfileDto userProfileDto);
        Task<bool> UpdateUserAsync(string id, UserUpdateDto userRequestDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
