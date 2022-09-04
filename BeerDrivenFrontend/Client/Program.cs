using BeerDrivenFrontend.Client;
using BeerDrivenFrontend.Modules.Production.Extensions;
using BeerDrivenFrontend.Modules.Pubs.Extensions;
using BeerDrivenFrontend.Shared.Configuration;
using BeerDrivenFrontend.Shared.Helpers;
using BlazorComponentBus;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using MudBlazor.Services;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Authentication
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    //options.ProviderOptions.DefaultAccessTokenScopes.Add("https://localhost:9043");
    //options.ProviderOptions.LoginMode = "redirect";
});
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
