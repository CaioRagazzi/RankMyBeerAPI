using RankMyBeerInfrastructure.Repositories.BeerRepository;
using RankMyBeerDomain.Models;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerService.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Google.Cloud.Storage.V1;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using RankMyBeerDomain.Entities;
using RankMyBeerApplication.Services.BeerPhotoService.Dtos;

namespace RankMyBeerApplication.Services.BeerServices;
public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public BeerService(IBeerRepository beerRepository, IMapper mapper, IConfiguration config)
    {
        _beerRepository = beerRepository;
        _mapper = mapper;
        _config = config;
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

        if (beerDtoRequest.BeerImageDtoRequests != null && beerDtoRequest.BeerImageDtoRequests.Any())
        {
            foreach (var item in beerDtoRequest.BeerImageDtoRequests)
            {
                if (item.Base64Photo is not null && item.ImageFileName is not null)
                {
                    await UploadPhoto(item.Base64Photo, item.ImageFileName, beer.Id);
                }
            }
        }

        var response = _mapper.Map<BeerDtoResponse>(beer);

        return response;
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
            foreach (var beer in item.BeerPhotos)
            {
                beer.PhotoURL = await CreateSignedURLGet(_config["BeerPhotoBucket:BucketName"], beer.UrlBucketName);
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

    private async Task UploadPhoto(string base64Photo, string fileName, Guid beerId)
    {
        var client = await StorageClient.CreateAsync();

        string base64WithoutHeader = Regex.Replace(base64Photo, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

        var content = Convert.FromBase64String(base64WithoutHeader);

        await client.UploadObjectAsync(
            _config["BeerPhotoBucket:BucketName"],
            $"{beerId}/beerPhoto/{fileName}",
            "text/plain",
            new MemoryStream(content)
        );
    }

    private async Task<string> CreateSignedURLGet(string bucketName, string objectName)
    {
        UrlSigner urlSigner = UrlSigner.FromCredentialFile(_config["BeerPhotoBucket:CredentialFilePath"]);
        return await urlSigner.SignAsync(bucketName, objectName, TimeSpan.FromHours(1), HttpMethod.Get);
    }
}
