using BeerDrivenFrontend.Modules.Pubs.Extensions.Dtos;

namespace BeerDrivenFrontend.Modules.Pubs.Extensions.Abstracts;

public interface IBeerService
{
    Task<IEnumerable<BeerJson>> GetBeersAsync();
}