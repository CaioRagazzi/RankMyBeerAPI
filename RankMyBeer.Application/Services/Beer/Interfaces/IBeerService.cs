using Microsoft.AspNetCore.JsonPatch;
using RankMyBeerApplication.Services.BeerService.Dtos;
using RankMyBeerDomain.Entities.Beer;
using RankMyBeerDomain.Models;

namespace RankMyBeerApplication.Services.BeerInterface.Interfaces;
public interface IBeerService
{
    Task<Beer> GetBeer(Guid id);
    Task<PagedResult<Beer>> GetBeer(string userId, int? page, int? pageSize);
    Task<Guid> AddBeer(BeerDtoRequest beerDtoRequest);
    Task PartialUpdate(Guid beerId, JsonPatchDocument<BeerDtoRequest> patchDoc);
    Task Update(Guid beerId, BeerDtoRequest beerDtoRequest);
    Task UploadPhoto(UploadBeerPhotoDtoRequest uploadBeerPhotoDtoRequest);
}
