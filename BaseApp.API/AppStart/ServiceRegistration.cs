using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.ServiceProvider.Services;
using BaseApp.Shared.Validation;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace BaseApp.API.AppStart
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add Validator
            services.AddSingleton<InputValidation>();

            // Add Providers
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();


            return services;
        }
    }
}
