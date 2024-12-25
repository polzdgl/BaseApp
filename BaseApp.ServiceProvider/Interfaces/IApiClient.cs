using BaseApp.Data.User.Dtos;
using BaseApp.Shared.Dtos;

namespace BaseApp.ServiceProvider.Interfaces
{
    public interface IApiClient
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> CreateUserAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> UpdateUserAsync(string id, UserRequestDto userRequestDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
