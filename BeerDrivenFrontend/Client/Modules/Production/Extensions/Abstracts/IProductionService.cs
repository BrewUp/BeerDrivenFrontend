using BeerDrivenFrontend.Client.Modules.Production.Extensions.Dtos;

namespace BeerDrivenFrontend.Client.Modules.Production.Extensions.Abstracts;

public interface IProductionService
{
    Task<IEnumerable<ProductionOrderJson>> GetProductionOrdersAsync();
    Task SendStartProductionOrderAsync(OrderJson order);
    Task SendCompleteProductionOrderAsync(OrderJson order);
}