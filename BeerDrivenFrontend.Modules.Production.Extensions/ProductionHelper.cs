using BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Production.Extensions.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace BeerDrivenFrontend.Modules.Production.Extensions;

public static class ProductionHelper
{
    public static IServiceCollection AddProductionModule(this IServiceCollection services)
    {
        services.AddScoped<IProductionService, ProductionService>();

        return services;
    }
}