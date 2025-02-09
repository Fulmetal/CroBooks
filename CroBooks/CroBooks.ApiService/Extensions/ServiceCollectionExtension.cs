using CroBooks.Domain.Companies;
using CroBooks.Domain.Interfaces;
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
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddScoped<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            return services;
        }
    }
}
