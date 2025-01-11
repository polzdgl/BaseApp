using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.User.Interfaces
{
    public interface IUserProvider
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> CreateUserAsync(UserProfileDto userProfileDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> EditUserAsync(string id, UserProfileDto userRequestDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
