using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Tests;
using CroBooks.Infrastructure;
using CroBooks.Infrastructure.Repositories;
using CroBooks.Services;
using CroBooks.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CroBooks.ApiService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("CroBooksDB");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(dbConnectionString);
            });

            services.AddTransient<Func<ApplicationDbContext?>>(provider => () => provider.GetService<ApplicationDbContext>());
            services.AddTransient<DbFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITestRepository, TestRepository>();

            return services;

        }
    }
}
