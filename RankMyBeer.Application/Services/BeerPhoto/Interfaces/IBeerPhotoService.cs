using RankMyBeerApplication.Services.BeerPhotoService.Dtos;

namespace RankMyBeerApplication.Services.BeerPhotoService;
public interface IBeerPhotoService
{
    Task<BeerPhotoDtoResponse> AddPhoto(BeerPhotoDtoRequest beerImageDtoRequest);
    Task RemoveBeerPhoto(Guid beerPhotoId);
    Task RemoveBeerPhotoByBeerId(Guid beerId);
}
