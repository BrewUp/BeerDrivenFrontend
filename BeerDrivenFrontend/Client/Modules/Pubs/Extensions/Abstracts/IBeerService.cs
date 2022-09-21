using BeerDrivenFrontend.Client.Modules.Pubs.Extensions.Dtos;

namespace BeerDrivenFrontend.Client.Modules.Pubs.Extensions.Abstracts;

public interface IBeerService
{
    Task<IEnumerable<BeerJson>> GetBeersAsync();
}