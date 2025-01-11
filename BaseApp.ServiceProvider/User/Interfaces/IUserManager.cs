using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.User.Interfaces
{
    public interface IUserManager
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize);
        Task<UserDto> GetUserAsync(string id);
        Task<bool> CreateUserAsync(UserProfileDto userProfileDto);
        Task<bool> UpdateUserAsync(string id, UserProfileDto userRequestDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
