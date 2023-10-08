using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Context;

namespace RankMyBeerInfrastructure.Repositories.UserRepository;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(RankMyBeerContext rankMyBeerContext) : base(rankMyBeerContext)
    {
    }
}
