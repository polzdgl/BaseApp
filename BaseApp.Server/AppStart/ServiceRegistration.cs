﻿using BaseApp.Data.Repositories;
using BaseApp.Data.Repositories.Interfaces;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.ServiceProvider.User.Manager;
using BaseApp.Shared.Validation;
using FluentValidation;

namespace BaseApp.Server.AppStart
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add Validator
            services.AddSingleton<InputValidation>();

            // Add fluent validation
            //builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name.StartsWith("BaseApp"))
                .ToArray();

            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

            // Add Providers
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();

            return services;
        }
    }
}
