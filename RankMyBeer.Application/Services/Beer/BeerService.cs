using RankMyBeerInfrastructure.Repositories.BeerRepository;
using RankMyBeerDomain.Models;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerService.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using RankMyBeerDomain.Entities;
using RankMyBeerApplication.Services.BeerPhotoService.Dtos;
using RankMyBeerApplication.Services.PhotoBucketService;
using RankMyBeerApplication.Services.BeerPhotoService;

namespace RankMyBeerApplication.Services.BeerServices;
public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoBucketService _photoBucketService;
    private readonly IBeerPhotoService _beerPhotoService;

    public BeerService(
        IBeerRepository beerRepository,
        IMapper mapper,
        IPhotoBucketService photoBucketService,
        IBeerPhotoService beerPhotoService)
    {
        _beerRepository = beerRepository;
        _mapper = mapper;
        _photoBucketService = photoBucketService;
        _beerPhotoService = beerPhotoService;
    }

    public async Task<Beer> GetBeer(Guid id)
    {
        var beer = await _beerRepository.GetByID(id);
        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        return beer;
    }

    public async Task<BeerDtoResponse> AddBeer(BeerDtoRequest beerDtoRequest)
    {
        var beer = _mapper.Map<Beer>(beerDtoRequest);
        beer.Id = Guid.NewGuid();

        await _beerRepository.Insert(beer);

        return _mapper.Map<BeerDtoResponse>(beer);
    }

    public async Task<PagedResult<BeerDtoResponse>> GetBeer(string userId, int? page, int? pageSize)
    {
        var pagedBeers = await _beerRepository.Get(
            page,
            pageSize,
            beer => beer.User == userId,
            beerOrd => beerOrd.OrderByDescending(beer => beer.Score),
            "BeerPhotos");

        var beerPhotoDtoResponseList = new List<BeerPhotoDtoResponse>();
        foreach (var item in pagedBeers.Results)
        {
            foreach (var beerPhoto in item.BeerPhotos)
            {
                beerPhoto.PhotoURL = await _photoBucketService.CreateSignedURLGet(beerPhoto.ImagePathBucket);
            }
        }
        var response = _mapper.Map<PagedResult<BeerDtoResponse>>(pagedBeers);

        return response;
    }

    public async Task PartialUpdate(Guid beerId, JsonPatchDocument<BeerDtoRequest> patchDoc)
    {
        var beer = await _beerRepository.GetByID(beerId);

        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        var JsonPatchDocument = _mapper.Map<JsonPatchDocument<Beer>>(patchDoc);

        JsonPatchDocument.ApplyTo(beer);

        await _beerRepository.SaveAsync();
    }

    public async Task Update(Guid beerId, BeerDtoRequest beerDtoRequest)
    {
        var beer = await _beerRepository.GetByID(beerId);

        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        var beerUpdated = _mapper.Map(beerDtoRequest, beer);

        await _beerRepository.Update(beerUpdated);
    }

    public async Task Delete(Guid beerId)
    {
        var beer = await _beerRepository.GetByID(beerId);

        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        await _beerPhotoService.RemoveBeerPhotoByBeerId(beerId);

        await _beerRepository.Delete(beer);
    }
}
