using Microsoft.AspNetCore.JsonPatch;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerDomain.Entities;
using RankMyBeerDomain.Models;

namespace RankMyBeerApplication.Services.BeerInterface.Interfaces;
public interface IBeerService
{
    Task<Beer> GetBeer(Guid id);
    Task<PagedResult<BeerDtoResponse>> GetBeer(string userId, int? page, int? pageSize);
    Task<BeerDtoResponse> AddBeer(BeerDtoRequest beerDtoRequest);
    Task PartialUpdate(Guid beerId, JsonPatchDocument<BeerDtoRequest> patchDoc);
    Task Update(Guid beerId, BeerDtoRequest beerDtoRequest);
}
