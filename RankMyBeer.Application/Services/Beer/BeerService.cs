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
using RankMyBeerInfrastructure.Repositories.BeerPhotoRepository;

namespace RankMyBeerApplication.Services.BeerServices;
public class BeerService : IBeerService
{
    private readonly IBeerRepository _beerRepository;
    private readonly IBeerPhotoRepository _beerPhotoRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public BeerService(
        IBeerRepository beerRepository,
        IBeerPhotoRepository beerPhotoRepository,
        IMapper mapper,
        IConfiguration config)
    {
        _beerRepository = beerRepository;
        _beerPhotoRepository = beerPhotoRepository;
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

        foreach (var item in beerDtoRequest.BeerPhotos)
        {
            var beerPhoto = await AddBeerPhoto(beer, item);
            beer.BeerPhotos.Add(beerPhoto);
        }

        await _beerRepository.Insert(beer);

        var response = _mapper.Map<BeerDtoResponse>(beer);

        return response;
    }

    private async Task<BeerPhoto> AddBeerPhoto(Beer beer, BeerImageDtoRequest beerImageDtoRequest)
    {
        var beerPhoto = new BeerPhoto
        {
            Id = Guid.NewGuid(),
            BeerId = beer.Id,
            ImageFileName = beerImageDtoRequest.ImageFileName
        };

        await UploadPhoto(beerImageDtoRequest.Base64Photo, beerImageDtoRequest.ImageFileName, beer.Id);
        return beerPhoto;
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
                beerPhoto.PhotoURL = await CreateSignedURLGet(_config["BeerPhotoBucket:BucketName"], beerPhoto.ImagePathBucket);
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

        foreach (var beerPhoto in beerDtoRequest.BeerPhotos)
        {
            if (beerPhoto.Id is null)
            {
                var beerPhotoToAdd = await AddBeerPhoto(beer, beerPhoto);
                await _beerPhotoRepository.Insert(beerPhotoToAdd);
                beer.BeerPhotos.Add(beerPhotoToAdd);
            }
            else if (!beer.BeerPhotos.Any(beerPhoto => beerPhoto.Id == beerPhoto.Id))
            {
                var result = await _beerPhotoRepository.Get(1, 1, beerPhoto => beerPhoto.BeerId == beer.Id);
                var beerPhotoToDelete = result.Results.FirstOrDefault();
                if (beerPhotoToDelete != null && beerPhotoToDelete.ImageFileName != null)
                {
                    await RemovePhoto(beerPhotoToDelete.ImageFileName, beer.Id);
                    await _beerPhotoRepository.Delete(beerPhotoToDelete);
                    beer.BeerPhotos.Remove(beerPhotoToDelete);
                }
            }
        }

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

    private async Task RemovePhoto(string fileName, Guid beerId)
    {
        var client = await StorageClient.CreateAsync();
        await client.DeleteObjectAsync(_config["BeerPhotoBucket:BucketName"], $"{beerId}/beerPhoto/{fileName}");
    }

    private async Task<string> CreateSignedURLGet(string bucketName, string objectName)
    {
        UrlSigner urlSigner = UrlSigner.FromCredentialFile(_config["BeerPhotoBucket:CredentialFilePath"]);
        return await urlSigner.SignAsync(bucketName, objectName, TimeSpan.FromHours(1), HttpMethod.Get);
    }
}
