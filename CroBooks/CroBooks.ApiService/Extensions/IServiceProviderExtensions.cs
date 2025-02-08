using CroBooks.Infrastructure;
//using CroBooks.Infrastructure.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CroBooks.ApiService.Extensions;

public static class ServiceProviderExtensions
{
    public async static Task MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();

            // now we have the DbContext. Run migrations
            context.Database.Migrate();

            // now that the database is up to date. Let's seed
            //new RoleSeeder(context).SeedData();

            await context.SaveChangesAsync();
        }
    }
}