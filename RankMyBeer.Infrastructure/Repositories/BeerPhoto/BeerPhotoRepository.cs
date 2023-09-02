using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Context;

namespace RankMyBeerInfrastructure.Repositories.BeerPhotoRepository;
public class BeerPhotoRepository : BaseRepository<BeerPhoto>, IBeerPhotoRepository
{
    public BeerPhotoRepository(RankMyBeerContext rankMyBeerContext) : base(rankMyBeerContext)
    {
    }
}
