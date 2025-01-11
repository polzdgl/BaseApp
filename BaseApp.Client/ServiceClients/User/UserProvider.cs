using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.Shared.Dtos;
using System.Net.Http.Json;

namespace BaseApp.Client.ServiceClients.User
{
    public partial class UserProvider : IUserProvider
    {
        private readonly HttpClient httpClient;

        public UserProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PaginatedResult<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<PaginatedResult<UserDto>>($"/api/user/users?page={page}&pageSize={pageSize}", cancellationToken);
                return response ?? new PaginatedResult<UserDto>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public async Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var userDto = await httpClient.GetFromJsonAsync<UserDto>($"/api/user/{id}", cancellationToken);
            if (userDto == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }
            return userDto;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserProfileDto userProfileDto, CancellationToken cancellationToken = default)
        {
            return await httpClient.PostAsJsonAsync($"/api/user", userProfileDto, cancellationToken);
        }

        public async Task<HttpResponseMessage> EditUserAsync(string id, UserProfileDto userProfileDto, CancellationToken cancellationToken = default)
        {
            return await httpClient.PutAsJsonAsync($"/api/user/{id}", userProfileDto, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            return await httpClient.DeleteAsync($"/api/user/{id}", cancellationToken);
        }

    }
}
