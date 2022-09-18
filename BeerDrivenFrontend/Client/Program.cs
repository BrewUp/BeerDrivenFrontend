using BeerDrivenFrontend.Client;
using BeerDrivenFrontend.Modules.Production.Extensions;
using BeerDrivenFrontend.Modules.Pubs.Extensions;
using BeerDrivenFrontend.Shared.Configuration;
using BeerDrivenFrontend.Shared.Helpers;
using BlazorComponentBus;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using MudBlazor.Services;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("BrewUpApiClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BrewUpApiClient"));

#region Authentication
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration["AzureAd:Scope"]);

    options.ProviderOptions.LoginMode = "redirect";
});

//builder.Services.AddBlazoradeMsal(options =>
//{
//    var config = builder.Configuration.GetSection("AzureAd");
//    options.ClientId = config.GetValue<string>("clientId");
//    options.TenantId = config.GetValue<string>("tenantId");

//    options.DefaultScopes = new[] { "User.Read" };
//    options.InteractiveLoginMode = InteractiveLoginMode.Popup;
//    options.TokenCacheScope = TokenCacheScope.Persistent;
//});
#endregion

#region Configuration
builder.Services.AddSingleton(_ => builder.Configuration.GetSection("BrewUp:AppConfiguration")
    .Get<AppConfiguration>());
builder.Services.AddApplicationService();
#endregion

#region Infrastructure
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs\\BrewUp.log")
    .CreateLogger();

builder.Services.AddMudServices();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<ComponentBus>();
#endregion

#region Modules
builder.Services.AddProductionModule();
builder.Services.AddPubsModule();
#endregion

await builder.Build().RunAsync();
