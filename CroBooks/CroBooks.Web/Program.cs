using Blazored.LocalStorage;
using CroBooks.Web.Components;
using CroBooks.Web.Extensions;
using CroBooks.Web.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClients();
builder.Services.AddSingleton<TranslationHelper>();

builder.Services.AddControllers();
builder.Services.AddLocalization();
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddOutputCache();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var cultures = builder.Configuration.GetSection("Cultures")
    .GetChildren().ToDictionary(x => x.Key, x=> x.Value);

string[] supportedCultures = cultures.Keys.ToArray();

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();