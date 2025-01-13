using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.Company.Interfaces;
using BaseApp.ServiceProvider.Company.Manager;
using BaseApp.ServiceProvider.Company.Porvider;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.ServiceProvider.User.Manager;
using BaseApp.Shared.Validations;
using FluentValidation;

namespace BaseApp.Server.AppStart
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add Input Validator
            services.AddSingleton<InputValidation>();
            
            // Add Fluent Validation
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()  
                .Where(assembly => assembly.GetName().Name.StartsWith("BaseApp"))
                .ToArray();
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

            // Add Repositories
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();

            // Add User Services
            services.AddTransient<IUserManager, UserManager>();

            // Add Security Exchange Services
            services.AddTransient<ICompanyManager, CompanyManager>();
            services.AddTransient<ISecurityExchangeProvider, SecurityExchangeProvider>();

            return services;
        }
    }
}
