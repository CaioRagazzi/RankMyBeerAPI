using System.Text.RegularExpressions;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace RankMyBeerApplication.Services.PhotoBucketService;
public class PhotoBucketService : IPhotoBucketService
{
    private readonly IConfiguration _config;
    private readonly IAmazonS3 _s3Client;

    public PhotoBucketService(IConfiguration config, IAmazonS3 s3Client)
    {
        _config = config;
        _s3Client = s3Client;
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

    public async Task<string> UploadPhotoAWS(string base64Photo, string fileName, Guid beerId)
    {
        var formatedBase64 = FormatImageBase64(base64Photo);
        var request = new PutObjectRequest()
        {
            BucketName = _config["BeerPhotoBucket:BucketName"],
            Key = $"{beerId}/beerPhoto/{fileName}",
            InputStream = new MemoryStream(Convert.FromBase64String(formatedBase64)),
            CannedACL = S3CannedACL.PublicRead
        };

        await _s3Client.PutObjectAsync(request);

        var url = CreateSignedURLGetAWS(request.Key);
        return url;
    }

    private string FormatImageBase64(string base64Photo)
    {
        Regex regex = new(@"^[\w/\:.-]+;base64,");
        return regex.Replace(base64Photo, string.Empty);
    }

    public async Task RemoveBeerPhoto(string fileName, Guid beerId)
    {
        await _s3Client.DeleteObjectAsync(_config["BeerPhotoBucket:BucketName"], $"{beerId}/beerPhoto/{fileName}");
    }

    public async Task<string> CreateSignedURLGet(string objectName)
    {
        UrlSigner urlSigner = UrlSigner.FromCredentialFile(_config["BeerPhotoBucket:CredentialFilePath"]);
        return await urlSigner.SignAsync(_config["BeerPhotoBucket:BucketName"], objectName, TimeSpan.FromHours(1), HttpMethod.Get);
    }

    public string CreateSignedURLGetAWS(string objectName)
    {
        const double timeoutDuration = 12;

        AWSConfigsS3.UseSignatureVersion4 = true;

        string urlString = GeneratePresignedURL(_config["BeerPhotoBucket:BucketName"] ?? "", objectName, timeoutDuration);
        return urlString;
    }

    public string GeneratePresignedURL(string bucketName, string objectKey, double duration)
    {
        string urlString = string.Empty;
        try
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddHours(duration),
            };
            urlString = _s3Client.GetPreSignedURL(request);
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error:'{ex.Message}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error:'{ex.Message}'");
        }

        return urlString;
    }
}
