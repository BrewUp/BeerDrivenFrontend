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

    public async Task<IEnumerable<BeerJson>> GetBeersAsync()
    {
        try
        {
            return await HttpService.Get<IEnumerable<BeerJson>>(
                $"{AppConfiguration.ProductionApiUri}production/beers");
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }
}