using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Context;

namespace RankMyBeerInfrastructure.Repositories.BeerRepository;
public class BeerRepository : BaseRepository<Beer>, IBeerRepository
{
    public BeerRepository(RankMyBeerContext context) : base(context)
    {
    }
}
