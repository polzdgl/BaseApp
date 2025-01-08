using BaseApp.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BaseApp.ServiceClients.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public AuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;

            try
            {
                // Make a request to the server to check if the user is authenticated
                var response = await _httpClient.GetAsync("api/auth/connectedUser");

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadFromJsonAsync<ConnectedUser>();

                    if (userInfo?.IsAuthenticated ?? false)
                    {
                        // Create claims based on the user information
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userInfo.UserName),
                            new Claim(ClaimTypes.Email, userInfo.Email),
                            // Add other claims as needed
                        };

                        identity = new ClaimsIdentity(claims, "serverAuth");
                    }
                    else
                    {
                        identity = new ClaimsIdentity();
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Handle the BadRequest response
                    Console.Error.WriteLine("The request was invalid. User is not authorized.");
                    identity = new ClaimsIdentity();
                }
                else
                {
                    // Handle other non-success status codes as needed
                    Console.Error.WriteLine($"Unexpected response status: {response.StatusCode}");
                    identity = new ClaimsIdentity();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"An error occurred while fetching user info: {ex.Message}");
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }


        public void NotifyUserAuthentication()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
