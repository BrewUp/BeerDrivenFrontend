using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;

namespace BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;

public interface IProductionService
{
    Task<IEnumerable<ProductionOrderJson>> GetProductionOrdersAsync();
    Task SendProductionOrderAsync(OrderJson order);
}