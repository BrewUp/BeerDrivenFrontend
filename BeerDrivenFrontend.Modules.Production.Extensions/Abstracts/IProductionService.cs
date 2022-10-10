using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;

namespace BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;

public interface IProductionService
{
    Task<IEnumerable<ProductionOrderJson>> GetProductionOrdersAsync();
    Task SendStartProductionOrderAsync(OrderJson order);
    Task SendCompleteProductionOrderAsync(OrderJson order);

    Task<IEnumerable<BeerLookupJson>> GetBeersAsync();
}