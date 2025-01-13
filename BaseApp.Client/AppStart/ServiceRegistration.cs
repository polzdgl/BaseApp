using BaseApp.Client.ServiceClients.Auth;
using BaseApp.Client.ServiceClients.Company;
using BaseApp.Client.ServiceClients.User;
using BaseApp.ServiceClients.Auth;
using BaseApp.ServiceProvider.Auth.Interfaces;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.ServiceProvider.User.Interfaces;
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
            services.AddScoped<IUserProvider, UserProvider>();

            // Add Auth Api Client
            services.AddScoped<IAuthClient, AuthClient>();

            // Add Company Api Client
            services.AddScoped<ICompanyClient, CompanyClient>();

            // Add Authentication State Provider
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            return services;
        }
    }
}
