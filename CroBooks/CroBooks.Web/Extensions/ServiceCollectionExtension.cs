using CroBooks.Web.HttpClients;

namespace CroBooks.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<CompanyHttpClient>(client =>
            { client.BaseAddress = new("https+http://apiservice/api/customer"); });

            services.AddHttpClient<UserHttpClient>(client =>
            { client.BaseAddress = new("https+http://apiservice/api/user"); });

            return services;
        }
    }
}
