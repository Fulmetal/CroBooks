﻿using CroBooks.Domain.Clients;
using CroBooks.Domain.Companies;
using CroBooks.Domain.Contacts;
using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Users;
using CroBooks.Infrastructure;
using CroBooks.Infrastructure.Repositories;
using CroBooks.Services;
using CroBooks.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using CroBooks.Domain.Roles;

namespace CroBooks.ApiService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string? dbConnectionString = configuration.GetConnectionString("CroBooksDB");

            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentNullException(nameof(dbConnectionString));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(dbConnectionString);
            });

            services.AddScoped<Func<ApplicationDbContext?>>(provider => () => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(dbConnectionString));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            return services;
        }
    }
}
