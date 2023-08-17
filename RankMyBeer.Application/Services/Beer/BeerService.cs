using RankMyBeerInfrastructure.Repositories.BeerRepository;
using RankMyBeerDomain.Entities.Beer;
using RankMyBeerDomain.Models;
using RankMyBeerApplication.Services.BeerInterface.Interfaces;
using RankMyBeerApplication.Services.BeerService.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Google.Cloud.Storage.V1;
using System.Text;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;

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

    public async Task<BeerDtoResponse> AddBeer(BeerDtoRequest beerDtoRequest)
    {
        var beer = _mapper.Map<Beer>(beerDtoRequest);

        beer.Id = Guid.NewGuid();

        await _beerRepository.Insert(beer);

        if (beerDtoRequest.Base64Photo != null && beerDtoRequest.ImageFileName != null)
        {
            var photoUrl = await UploadPhoto(beerDtoRequest.Base64Photo, beerDtoRequest.ImageFileName, beer.Id);
            beer.PhotoURL = photoUrl;
            await _beerRepository.Update(beer);
        }

        var response = _mapper.Map<BeerDtoResponse>(beer);

        return response;
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

    private async Task<string> UploadPhoto(string base64Photo, string fileName, Guid beerId)
    {
        var client = await StorageClient.CreateAsync();

        var bucket = await client.GetBucketAsync("rankmybeer.appspot.com");

        string base64WithoutHeader = Regex.Replace(base64Photo, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

        var content = Convert.FromBase64String(base64WithoutHeader);

        var teste = await client.UploadObjectAsync(
            bucket.Name,
            $"{beerId}/beerPhoto/{fileName}",
            "text/plain",
            new MemoryStream(content)
        );

        return await CreateSignedURLGet(bucket.Name, teste.Name);
    }

    public async Task<string> CreateSignedURLGet(string bucketName, string objectName)
    {
        var credential = GoogleCredential.GetApplicationDefault();
        UrlSigner urlSigner = UrlSigner.FromCredential(credential);
        return await urlSigner.SignAsync(bucketName, objectName, TimeSpan.FromHours(1), HttpMethod.Get);
    }
}
