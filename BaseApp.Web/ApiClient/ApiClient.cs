using BaseApp.Data.User.Dtos;
using System.Net.Http.Json;

namespace BaseApp.Web.ServiceClients
{
    public class ApiClient(HttpClient httpClient)
    {
        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            var userDtos = await httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("/user/users", cancellationToken = default);

            return userDtos ?? Enumerable.Empty<UserDto>();
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
            var response = await httpClient.PostAsJsonAsync<UserRequestDto>($"/user", userRequestDto, cancellationToken);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(string id, UserRequestDto userRequestDto, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PutAsJsonAsync<UserRequestDto>($"/user/{id}", userRequestDto, cancellationToken);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.DeleteAsync($"/user/{id}", cancellationToken);
            return response;
        }
    }
}
