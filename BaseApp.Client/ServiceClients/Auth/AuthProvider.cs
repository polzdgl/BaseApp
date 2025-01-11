using BaseApp.Data.User.Dtos;
using BaseApp.ServiceClients.Auth;
using BaseApp.ServiceProvider.Auth.Interfaces;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BaseApp.Client.ServiceClients.Auth
{
    public class AuthProvider : IAuthProvider
    {
        private readonly HttpClient httpClient;
        private readonly InputValidation inputValidation;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthProvider(HttpClient httpClient, InputValidation inputValidation, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.inputValidation = inputValidation;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default)
        {
            // Register User
            //var passwordValidationResults = inputValidation.ValidatePassword(userRegisterDto.Password, out var passwordValidationErrors);

            //if (!passwordValidationResults)
            //{
            //    // Create a bad request response with the validation errors
            //    var errorResponse = new
            //    {
            //        Message = "Password validation failed.",
            //        Errors = passwordValidationErrors.Select(vr => vr.ErrorMessage).FirstOrDefault()
            //    };

            //    var errorContent = new StringContent(
            //        System.Text.Json.JsonSerializer.Serialize(passwordValidationErrors.Select(vr => vr.ErrorMessage).FirstOrDefault()),
            //        System.Text.Encoding.UTF8,
            //        "application/json");

            //    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            //    {
            //        Content = errorContent
            //    };
            //}

            return await httpClient.PostAsJsonAsync($"/api/auth/register", userRegisterDto, cancellationToken);
        }

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
            var loginResult = await httpClient.PostAsJsonAsync($"/login{query}", payload, cancellationToken);

            if (loginResult.IsSuccessStatusCode)
            {
                // Update AuthStateProvider with the new user state
                var authProvider = (AuthStateProvider)authenticationStateProvider;
                authProvider.NotifyUserAuthentication();
                return loginResult;
            }
            else
            {
                return loginResult;
            }
        }

        public async Task<HttpResponseMessage> LogoutAsync(CancellationToken cancellationToken = default)
        {
            return await httpClient.PostAsync("api/auth/logout", null, cancellationToken);
        }
    }
}
