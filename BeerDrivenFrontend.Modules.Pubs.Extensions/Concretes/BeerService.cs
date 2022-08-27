using BeerDrivenFrontend.Modules.Pubs.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Pubs.Extensions.Dtos;
using BeerDrivenFrontend.Shared.Abstracts;
using BeerDrivenFrontend.Shared.Concretes;
using BeerDrivenFrontend.Shared.Configuration;
using Microsoft.Extensions.Logging;

namespace BeerDrivenFrontend.Modules.Pubs.Extensions.Concretes;

public sealed class BeerService : BaseHttpService, IBeerService
{
    public BeerService(HttpClient httpClient,
        IHttpService httpService,
        AppConfiguration appConfiguration,
        ILoggerFactory loggerFactory) : base(httpClient, httpService, appConfiguration, loggerFactory)
    {
    }

    public async Task<IEnumerable<BeerJson>> GetBeersAsync()
    {
        try
        {
            return await HttpService.Get<IEnumerable<BeerJson>>(
                $"{AppConfiguration.PubsApiUri}v1/pubs/beers");
        }
        catch (Exception ex)
        {
            Logger.LogError(CommonServices.GetDefaultErrorTrace(ex));
            throw;
        }
    }
}