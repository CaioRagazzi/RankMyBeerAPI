namespace RankMyBeerApplication.Services.PhotoBucketService;
public interface IPhotoBucketService
{
    Task UploadPhoto(string base64Photo, string fileName, Guid beerId);
    Task RemoveBeerPhoto(string fileName, Guid beerId);
    Task<string> CreateSignedURLGet(string objectName);
}