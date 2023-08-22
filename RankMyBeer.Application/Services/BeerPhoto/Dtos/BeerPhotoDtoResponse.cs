namespace RankMyBeerApplication.Services.BeerPhotoService.Dtos;
public class BeerPhotoDtoResponse
{
    public Guid Id { get; set; }
    public Guid BeerId { get; set; }
    public string? ImageFileName { get; set; }
    public string? PhotoURL { get; set; }
}
