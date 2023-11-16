using RankMyBeerApplication.Services.BeerPhotoService.Dtos;
using RankMyBeerApplication.Services.PhotoBucketService;
using RankMyBeerDomain.Entities;
using RankMyBeerInfrastructure.Repositories.BeerPhotoRepository;
using RankMyBeerInfrastructure.Repositories.BeerRepository;

namespace RankMyBeerApplication.Services.BeerPhotoService;
public class BeerPhotoService : IBeerPhotoService
{
    private readonly IBeerPhotoRepository _beerPhotoRepository;
    private readonly IBeerRepository _beerRepository;
    private readonly IPhotoBucketService _photoBucketService;

    public BeerPhotoService(
        IBeerPhotoRepository beerPhotoRepository,
        IBeerRepository beerRepository,
        IPhotoBucketService photoBucketService)
    {
        _beerPhotoRepository = beerPhotoRepository;
        _beerRepository = beerRepository;
        _photoBucketService = photoBucketService;
    }

    public async Task AddPhoto(BeerPhotoDtoRequest beerImageDtoRequest)
    {
        var beer = await _beerRepository.GetByID(beerImageDtoRequest.BeerId);

        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        var beerPhotoToAdd = await AddBeerPhoto(beer, beerImageDtoRequest);
        await _beerPhotoRepository.Insert(beerPhotoToAdd);
    }

    private async Task<BeerPhoto> AddBeerPhoto(Beer beer, BeerPhotoDtoRequest beerImageDtoRequest)
    {
        var beerPhoto = new BeerPhoto
        {
            Id = Guid.NewGuid(),
            BeerId = beer.Id,
            ImageFileName = beerImageDtoRequest.ImageFileName
        };

        await _photoBucketService.UploadPhotoAWS(beerImageDtoRequest.Base64Photo, beerImageDtoRequest.ImageFileName, beer.Id);
        return beerPhoto;
    }

    public async Task RemoveBeerPhoto(Guid beerPhotoId)
    {
        var beerPhoto = await _beerPhotoRepository.GetByID(beerPhotoId);

        if (beerPhoto == null)
            throw new InvalidOperationException("Beer Photo does not exists");

        await _photoBucketService.RemoveBeerPhoto(beerPhoto.ImageFileName, beerPhoto.BeerId);
        await _beerPhotoRepository.Delete(beerPhoto);
    }

    public async Task RemoveBeerPhotoByBeerId(Guid beerId)
    {
        var beerPhotos = await _beerPhotoRepository.Get(page: 1, pageSize: 1, filter: x => x.BeerId == beerId);

        foreach (var beerPhoto in beerPhotos.Results)
        {
            await RemoveBeerPhoto(beerPhoto.Id);
        }
    }
}