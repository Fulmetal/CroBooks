using CroBooks.Web.HttpClients;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IApiHttpClientBase, ApiHttpClientBase>("ServerApi", client =>
            { client.BaseAddress = new("https+http://apiservice"); });

            services.AddScoped(
                sp => sp.GetService<IHttpClientFactory>()!.CreateClient("ServerApi"));

            services.AddScoped<UserHttpClient, UserHttpClient>();
            services.AddScoped<CompanyHttpClient, CompanyHttpClient>();
            services.AddScoped<AuthHttpClient, AuthHttpClient>();
            //services.AddHttpClient<UserHttpClient>(client =>
            ////{ client.BaseAddress = new("https+http://ApiService/api/user"); });
            //{ client.BaseAddress = new("https+http://apiservice"); });

            return services;
        }
    }
}
