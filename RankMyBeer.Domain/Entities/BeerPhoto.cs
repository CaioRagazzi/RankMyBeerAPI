namespace RankMyBeerDomain.Entities;
public class BeerPhoto
{
    public Guid Id { get; set; }
    public Guid BeerId { get; set; }
    public string? ImageFileName { get; set; }
    public string? PhotoURL { get; set; }
    public string UrlBucketName
    {
        get
        {
            return $"{BeerId}/beerPhoto/{ImageFileName}";
        }
    }
    public required Beer Beer { get; set; }
}
