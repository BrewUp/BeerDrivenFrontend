using BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using BeerDrivenFrontend.Shared.Abstracts;
using BeerDrivenFrontend.Shared.Concretes;
using BeerDrivenFrontend.Shared.Configuration;
using Microsoft.Extensions.Logging;

namespace BeerDrivenFrontend.Modules.Production.Extensions.Concretes;

public sealed class ProductionService : BaseHttpService, IProductionService
{
    public ProductionService(HttpClient httpClient, IHttpService httpService, AppConfiguration appConfiguration,
        ILoggerFactory loggerFactory) : base(httpClient, httpService, appConfiguration, loggerFactory)
    {
    }

    public async Task<IEnumerable<ProductionOrderJson>> GetProductionOrdersAsync()
    {
        try
        {
            return await HttpService.Get<IEnumerable<ProductionOrderJson>>(
                $"{AppConfiguration.ProductionApiUri}v1/production");
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }

    public async Task SendStartProductionOrderAsync(OrderJson order)
    {
        try
        {
            await HttpService.Post($"{AppConfiguration.ProductionApiUri}v1/production/beers/brew", order);
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }

    public async Task SendCompleteProductionOrderAsync(OrderJson order)
    {
        try
        {
            await HttpService.Put($"{AppConfiguration.ProductionApiUri}v1/production/beers/brew/{order.BatchNumber}",
                order);
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }

    public async Task<IEnumerable<BeerLookupJson>> GetBeersAsync()
    {
        try
        {
            return await HttpService.Get<IEnumerable<BeerLookupJson>>(
                $"{AppConfiguration.ProductionApiUri}v1/production/beers");
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }
}