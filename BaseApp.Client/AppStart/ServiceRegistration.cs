using BaseApp.Client.ServiceClients.Auth;
using BaseApp.Client.ServiceClients.User;
using BaseApp.ServiceClients.Auth;
using BaseApp.ServiceProvider.Interfaces.Auth;
using BaseApp.ServiceProvider.Interfaces.User;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.Components.Authorization;

namespace BaseApp.Client.AppStart
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add Validator
            services.AddSingleton<InputValidation>();

            // Add User Api Client
            services.AddScoped<IUserApiClient, UserApiClient>();

            // Add Auth Api Client
            services.AddScoped<IAuthApiClient, AuthApiClient>();

            // Add Authentication State Provider
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            return services;
        }
    }
}
