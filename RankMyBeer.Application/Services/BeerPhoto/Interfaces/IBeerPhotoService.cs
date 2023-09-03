using RankMyBeerApplication.Services.BeerPhotoService.Dtos;

namespace RankMyBeerApplication.Services.BeerPhotoService;
public interface IBeerPhotoService
{
    Task AddPhoto(Guid beerId, BeerPhotoDtoRequest beerImageDtoRequest);
    Task RemoveBeerPhoto(Guid beerPhotoId);
}
