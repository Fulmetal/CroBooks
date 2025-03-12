using CroBooks.Web.Helpers;
using CroBooks.Web.HttpClients;
using CroBooks.Web.HttpClients.Base;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace CroBooks.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IApiHttpClientBase, ApiHttpClientBase>("ServerApi", (sp, cl) =>
            { cl.BaseAddress = new("https+http://apiservice"); cl.EnableIntercept(sp); });
                //.AddHttpMessageHandler<CustomHttpClientHandler>();

            services.AddScoped(
                sp => sp.GetService<IHttpClientFactory>()!.CreateClient("ServerApi"));

            services.AddHttpClientInterceptor();
            services.AddTransient<HttpInterceptorService>();
            //services.AddTransient<CustomHttpClientHandler>();


            services.AddScoped<UserHttpClient, UserHttpClient>();
            services.AddScoped<CompanyHttpClient, CompanyHttpClient>();
            services.AddScoped<AuthHttpClient, AuthHttpClient>();

            return services;
        }
    }
}
