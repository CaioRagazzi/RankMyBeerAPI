using System.Text.RegularExpressions;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace RankMyBeerApplication.Services.PhotoBucketService;
public class PhotoBucketService : IPhotoBucketService
{
    private readonly IConfiguration _config;

    public PhotoBucketService(IConfiguration config)
    {
        _config = config;
    }

    public async Task UploadPhoto(string base64Photo, string fileName, Guid beerId)
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

    public async Task RemoveBeerPhoto(string fileName, Guid beerId)
    {
        var client = await StorageClient.CreateAsync();
        await client.DeleteObjectAsync(_config["BeerPhotoBucket:BucketName"], $"{beerId}/beerPhoto/{fileName}");
    }

    public async Task<string> CreateSignedURLGet(string objectName)
    {
        UrlSigner urlSigner = UrlSigner.FromCredentialFile(_config["BeerPhotoBucket:CredentialFilePath"]);
        return await urlSigner.SignAsync(_config["BeerPhotoBucket:BucketName"], objectName, TimeSpan.FromHours(1), HttpMethod.Get);
    }
}
