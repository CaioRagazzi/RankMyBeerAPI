using RankMyBeerInfrastructure.Repositories.BeerRepository;
using RankMyBeerDomain.Entities.Beer;
using RankMyBeerDomain.Models;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerService.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Google.Cloud.Storage.V1;
using System.Text;

namespace RankMyBeerApplication.Services.BeerServices;
public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;
    private readonly IMapper _mapper;

    public BeerService(IBeerRepository beerRepository, IMapper mapper)
    {
        _beerRepository = beerRepository;
        _mapper = mapper;
    }

    public async Task<Beer> GetBeer(Guid id)
    {
        var beer = await _beerRepository.GetByID(id);
        if (beer == null)
            throw new InvalidOperationException("Beer does not exists");

        return beer;
    }

    public async Task<Guid> AddBeer(BeerDtoRequest beerDtoRequest)
    {
        var beer = _mapper.Map<Beer>(beerDtoRequest);

        beer.Id = Guid.NewGuid();

        await _beerRepository.Insert(beer);

        return beer.Id;
    }

    public async Task<PagedResult<Beer>> GetBeer(string userId, int? page, int? pageSize)
    {
        var pagesBeers = await _beerRepository.Get(
            page,
            pageSize,
            beer => beer.User == userId,
            beerOrd => beerOrd.OrderByDescending(beer => beer.Score));

        return pagesBeers;
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

    public async Task UploadPhoto(UploadBeerPhotoDtoRequest uploadBeerPhotoDtoRequest)
    {
        var client = await StorageClient.CreateAsync();

        var bucket = await client.GetBucketAsync("rankmybeer.appspot.com");


        var content = Convert.FromBase64String(uploadBeerPhotoDtoRequest.Base64Photo);
        var uploadedFile = await client.UploadObjectAsync(
            bucket.Name,
            $"{uploadBeerPhotoDtoRequest.BeerId}/beerPhoto/{uploadBeerPhotoDtoRequest.FileName}",
            "text/plain",
            new MemoryStream(content)
        );
    }
}
