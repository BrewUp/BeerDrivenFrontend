using BeerDrivenFrontend.Modules.Pubs.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Pubs.Extensions.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace BeerDrivenFrontend.Modules.Pubs.Extensions;

public static class PubsHelper
{
    public static IServiceCollection AddPubsModule(this IServiceCollection services)
    {
        services.AddScoped<IBeerService, BeerService>();

        return services;
    }
}