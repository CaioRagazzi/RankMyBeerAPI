using RankMyBeerInfrastructure.Repositories.BeerRepository;
using RankMyBeerDomain.Entities.Beer;
using RankMyBeerApplication.BeerInterface.Interfaces;
using RankMyBeerDomain.Models;

namespace RankMyBeerApplication.BeerServices;
public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;
    public BeerService(IBeerRepository beerRepository)
    {
        _beerRepository = beerRepository;
    }

    public async Task<Beer> GetBeer(Guid id)
    {
        var beer = await _beerRepository.GetByID(id);
        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        return beer;
    }

    public async Task AddBeer(Beer beer)
    {
        await _beerRepository.Insert(beer);
    }

    public async Task<PagedResult<Beer>> GetBeer(string userId, int page, int pageSize)
    {
        var pagesBeers = await _beerRepository.Get(page, pageSize, beer => beer.User == userId);

        return pagesBeers;
    }
}
