using RankMyBeerApplication.Services.BeerServices;

namespace RankMyBeer.Test.Services.BeerTest;
public class BeerServiceTest : BaseTest
{
    private readonly BeerService _beerService;
    public BeerServiceTest()
    {
        _beerService = _mocker.CreateInstance<BeerService>();
    }
}
