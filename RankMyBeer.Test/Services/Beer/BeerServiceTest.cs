using AutoMapper;
using FluentAssertions;
using Moq;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerApplication.Services.BeerServices;
using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Repositories.BeerRepository;
using Xunit;

namespace RankMyBeer.Test.Services.BeerTest;
public class BeerServiceTest : BaseTest
{
    private readonly BeerService _beerService;
    private readonly BeerDtoRequest _beerDtoRequest;
    private readonly BeerDtoResponse _beerDtoResponse;
    private readonly Beer _beer;

    public BeerServiceTest()
    {
        _beerService = _mocker.CreateInstance<BeerService>();
        _beerDtoRequest = CreateFixture<BeerDtoRequest>();

        _beer = new Beer
        {
            Name = "Beer",
            Brand = "Brand",
            Score = 1,
            User = "User"
        };

        _mocker.GetMock<IMapper>().Setup(m => m.Map<Beer>(_beerDtoRequest)).Returns(_beer);
        _mocker.GetMock<IBeerRepository>().Setup(m => m.Insert(_beer));

        _beerDtoResponse = CreateFixture<BeerDtoResponse>();
        _mocker.GetMock<IMapper>().Setup(m => m.Map<BeerDtoResponse>(_beer)).Returns(_beerDtoResponse);
    }

    [Fact]
    public async Task ItShouldRunWithoutErrors()
    {
        Func<Task> act = () => _beerService.AddBeer(_beerDtoRequest);
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ItShouldMapBeerDtoRequestToBeer()
    {
        await _beerService.AddBeer(_beerDtoRequest);

        _mocker.GetMock<IMapper>().Verify(m => m.Map<Beer>(_beerDtoRequest), Times.Once);
    }

    [Fact]
    public async Task BeerIdShouldHaveAGUIDAssigned()
    {
        await _beerService.AddBeer(_beerDtoRequest);

        _beer.Id.Should().NotBeEmpty();
    }
}
