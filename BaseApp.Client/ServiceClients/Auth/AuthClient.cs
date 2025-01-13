using BaseApp.Data.User.Dtos;
using BaseApp.ServiceClients.Auth;
using BaseApp.ServiceProvider.Auth.Interfaces;
using BaseApp.Shared.Validations;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BaseApp.Client.ServiceClients.Auth
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly InputValidation _inputValidation;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthClient(HttpClient httpClient, InputValidation inputValidation, AuthenticationStateProvider authenticationStateProvider)
        {
            this._httpClient = httpClient;
            this._inputValidation = inputValidation;
            this._authenticationStateProvider = authenticationStateProvider;
        }

        // Call the Register endpoint on the server to register a new user
        public async Task<HttpResponseMessage> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync($"/api/auth/register", userRegisterDto, cancellationToken);
        }

        // Call the Login endpoint on the server to log in a user
        public async Task<HttpResponseMessage> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default)
        {
            if (userLoginDto == null)
            {
                throw new ArgumentNullException(nameof(userLoginDto), "User login data cannot be null.");
            }

            var useCookies = userLoginDto.RememberMe ? true : false;
            var useSessionCookies = true; // default is true for session cookies

            // Construct the query string with query parameters
            var query = $"?useCookies={useCookies.ToString().ToLower()}&useSessionCookies={useSessionCookies}";

            // Prepare the request payload
            var payload = new
            {
                email = userLoginDto.Email,
                password = userLoginDto.Password
            };

            // Make the POST request with the constructed URL and payload
            var loginResult = await _httpClient.PostAsJsonAsync($"/login{query}", payload, cancellationToken);

            if (loginResult.IsSuccessStatusCode)
            {
                // Update AuthStateProvider with the new user state
                var authProvider = (AuthStateProvider)_authenticationStateProvider;
                authProvider.NotifyUserAuthentication();
                return loginResult;
            }
            else
            {
                return loginResult;
            }
        }

        // Todo: Implement the LogoutAsync method
        public async Task<HttpResponseMessage> LogoutAsync(CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsync("api/auth/logout", null, cancellationToken);
        }
    }
}
