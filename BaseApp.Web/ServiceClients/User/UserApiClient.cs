using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Dtos;
using System.Net.Http;
using System.Net.Http.Json;

namespace BaseApp.Web.ServiceClients
{
    public partial class UserApiClient(HttpClient httpClient) : IUserApiClient
    {
        public async Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetFromJsonAsync<PaginatedResult<UserDto>>($"/user/users?page={page}&pageSize={pageSize}", cancellationToken);
            return response ?? new PaginatedResult<UserDto>();
        }

        public async Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var userDto = await httpClient.GetFromJsonAsync<UserDto>($"/user/{id}", cancellationToken);
            if (userDto == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }
            return userDto;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken = default)
        {
            return await httpClient.PostAsJsonAsync($"/user", userRequestDto, cancellationToken);
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(string id, UserRequestDto userRequestDto, CancellationToken cancellationToken = default)
        {
            return await httpClient.PutAsJsonAsync($"/user/{id}", userRequestDto, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            return await httpClient.DeleteAsync($"/user/{id}", cancellationToken);
        }
    }
}
