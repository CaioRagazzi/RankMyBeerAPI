using RankMyBeerDomain.Entities.Beer;
using RankMyBeerDomain.Models;

namespace RankMyBeerApplication.BeerInterface.Interfaces;
public interface IBeerService
{
    Task<Beer> GetBeer(Guid id);
    Task<PagedResult<Beer>> GetBeer(string userId, int page, int pageSize);
    Task AddBeer(Beer beer);
}
