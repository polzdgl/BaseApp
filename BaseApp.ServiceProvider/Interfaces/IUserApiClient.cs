using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IUserApiClient
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> CreateUserAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> EditUserAsync(string id, UserRequestDto userRequestDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
