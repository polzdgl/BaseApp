using BaseApp.Data.User.Dtos;
using System.Net.Http.Json;

namespace BaseApp.Web.ServiceClients
{
    public class ApiClient(HttpClient httpClient)
    {
        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            var userDtos = await httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("/user", cancellationToken = default);

            return userDtos ?? Enumerable.Empty<UserDto>();
        }

        public async Task<UserDto> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var userDto = await httpClient.GetFromJsonAsync<UserDto>($"/user/{id}", cancellationToken);

            if (userDto == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            return userDto;
        }


        public async Task<bool> CreateUserAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync<UserRequestDto>($"/user", userRequestDto, cancellationToken);

            // Check if the request was successful, or throw error
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, UserRequestDto userRequestDto, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PutAsJsonAsync<UserRequestDto>($"/user/{id}", userRequestDto, cancellationToken);

            // Check if the request was successful, or throw error
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.DeleteAsync($"/user/{id}", cancellationToken);

            // Check if the request was successful, or throw error
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
